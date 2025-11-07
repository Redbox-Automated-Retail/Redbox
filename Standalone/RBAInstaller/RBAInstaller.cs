using DeviceService.ComponentModel;
using DeviceService.ComponentModel.FileUpdate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace RBAInstaller
{
    public class RBAInstaller : IHostedService
    {
        private ILogger<RBAInstaller> _logger;
        private IIUC285Proxy _proxy;
        private IFileUpdater _fileUpdater;
        private IConfiguration _configuration;
        private const string UPDATE_FILE = "update.json";
        private const string DEPLOYMENT_FOLDER = "RBA Files";

        public RBAInstaller(
            ILogger<RBAInstaller> logger,
            IIUC285Proxy proxy,
            IFileUpdater fileUpdater,
            IConfiguration configuration)
        {
            _logger = logger;
            _proxy = proxy;
            _fileUpdater = fileUpdater;
            _configuration = configuration;
        }

        private void Log(string message)
        {
            var logger = _logger;
            if (logger == null)
                return;
            LoggerExtensions.LogInformation((ILogger)logger, message, Array.Empty<object>());
        }

        private bool WaitForConnection()
        {
            if (_proxy.IsConnected)
            {
                Log("Already Connected - Continue.");
                return true;
            }

            Log("Wait For Connection To Device.");
            var mre = new ManualResetEvent(false);
            _proxy.OnConnectedHandler += (OnConnected)(() => mre.Set());
            return mre.WaitOne(180000);
        }

        private bool IsQA()
        {
            return _configuration.GetChildren().Any(x => x.Key == "QA");
        }

        private static string GetFilePath(string fileName)
        {
            return Path.Combine(Path.Combine(Environment.CurrentDirectory, "RBA Files"), fileName);
        }

        private UpdateJson LoadUpdateFile()
        {
            var filePath = GetFilePath("update.json");
            if (!File.Exists(filePath))
            {
                Log("Unable to locate: " + filePath);
                return null;
            }

            var updateJson = (UpdateJson)null;
            try
            {
                var message = File.ReadAllText(filePath);
                Log(message);
                updateJson = JsonConvert.DeserializeObject<UpdateJson>(message, new JsonSerializerSettings
                {
                    MissingMemberHandling = 0
                });
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError((ILogger)_logger, ex, "Unhandled exception in LoadUpdateFile",
                    (object[])null);
            }

            return updateJson;
        }

        private bool UpdateRBA(UpdateFile file)
        {
            if (_proxy?.UnitData == null)
            {
                Log("Update RBA Missing Unit Data");
                return false;
            }

            var str = (string)null;
            if (!string.IsNullOrWhiteSpace(file.MinorVersion))
                str = _proxy.GetVariable_29("254");
            Log(
                $"Update RBA Version: {file.CompareVersion} - Minor Version: {file.MinorVersion} - Current Version: {_proxy.UnitData.ApplicationVersion} - Current Minor Version: {str}");
            if (!(_proxy.UnitData.ApplicationVersion == file.CompareVersion) ||
                (str != null && !(file.MinorVersion == str)))
                return WriteFile(file, true);
            Log("Update RBA Currently Up To Date.");
            return true;
        }

        private bool UpdateRKI(UpdateFile file)
        {
            var proxy = _proxy;
            Log($"Begin UpdateRKI - {(ValueType)(proxy != null ? proxy.HasValidVasRKI ? 1 : 0 : 0)}");
            if (_proxy == null)
            {
                Log("UpdateRKI failed due to missing Proxy injection.");
                return false;
            }

            if (_proxy.HasValidVasRKI)
            {
                Log("UpdateRKI already supports VAS");
                return true;
            }

            Log(
                $"UpdateRKI - File IsIntermediateVasKey: {file.IsIntermediateVasKey} - CardReader HasInvalidVasRKI: {_proxy.HasInvalidVasRKI} - HasIntermediateVasKey: {_proxy.HasIntermediateVasRepairKey}");
            if ((!file.IsIntermediateVasKey || _proxy.HasInvalidVasRKI) &&
                (file.IsIntermediateVasKey || !_proxy.HasInvalidVasRKI))
                return WriteFile(file, true);
            Log("Skiping File: " + file.Path);
            return true;
        }

        private bool WriteFile(UpdateFile file, bool reboot)
        {
            if (string.IsNullOrWhiteSpace(file?.Path))
            {
                Log("Write File Failed - No File Specified");
                return false;
            }

            var filePath = GetFilePath(file.Path);
            if (!File.Exists(filePath))
            {
                Log("Write File Failed - Unable to locate: " + filePath);
                return false;
            }

            try
            {
                return _fileUpdater.WriteStream((Stream)File.OpenRead(filePath), file.Path, reboot, true, 180000)
                    .Result;
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError((ILogger)_logger, ex, "Unhandled Exception During WriteFile.",
                    Array.Empty<object>());
                return false;
            }
        }

        private bool BeginUpdates()
        {
            try
            {
                Log("Begin Updates");
                var updateJson = LoadUpdateFile();
                if (updateJson?.Files == null || !updateJson.Files.Any<UpdateFile>())
                {
                    Log("Unable to load update files");
                    return false;
                }

                var flag = true;
                foreach (var file in updateJson.Files)
                {
                    var upper = Path.GetExtension(file.Path).ToUpper();
                    Log($"File Path: {file.Path} - Compare Version: {file.CompareVersion}");
                    Log($"Connected: {WaitForConnection()}");
                    switch (upper)
                    {
                        case ".OGZ":
                            flag = UpdateRBA(file);
                            break;
                        case ".RKI":
                            flag = UpdateRKI(file);
                            break;
                    }

                    if (!flag)
                        break;
                }

                Log($"Updates Complete Success: {flag}");
                if (flag)
                {
                    WaitForConnection();
                    var updateRevisionNumber = _fileUpdater.GetFileUpdateRevisionNumber();
                    Log($"Update Revision to {updateJson.Revision} from {updateRevisionNumber}");
                    int result;
                    if (int.TryParse(updateJson.Revision, out result) && result > updateRevisionNumber)
                        _fileUpdater.SetFileUpdateRevisionNumber(updateJson.Revision);
                    else if (result == updateRevisionNumber)
                        Log("Revision Already Up To Date");
                    else
                        LoggerExtensions.LogError((ILogger)_logger, "Failed to update revision.",
                            Array.Empty<object>());
                }

                return flag;
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError((ILogger)_logger,
                    $"Unhandled Exception in BeginUpdates: {ex.Message}\n{ex.InnerException}\n{ex.StackTrace}",
                    Array.Empty<object>());
                return false;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log($"Connected = {WaitForConnection()}");
            Log("Update Files");
            BeginUpdates();
            Log("Update Complete");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}