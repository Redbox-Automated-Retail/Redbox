using DeviceService.ComponentModel;
using DeviceService.ComponentModel.FileUpdate;
using DeviceService.ComponentModel.KDS;
using DeviceService.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RBA_SDK;
using RBA_SDK_ComponentModel;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Threading;

namespace RBAInstaller
{
    internal class Program
    {
        private const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";

        private static void BeginInstall(IFileUpdateService fus)
        {
            fus.UpdateFiles();
        }

        private static IConfiguration GetConfiguration()
        {
            return (IConfiguration)JsonConfigurationExtensions
                .AddJsonFile(
                    FileConfigurationExtensions.SetBasePath((IConfigurationBuilder)new ConfigurationBuilder(),
                        Directory.GetCurrentDirectory()), "rbainstallersettings.json", false, true).Build();
        }

        private static ServiceProvider RegisterServices(IServiceCollection services)
        {
            ServiceCollectionServiceExtensions.AddSingleton(services, Log.Logger);
            ServiceCollectionServiceExtensions.AddSingleton<IRBA_API, RBA_API>(services);
            var singletonIUC285Proxy = (IIUC285Proxy)null;
            ServiceCollectionServiceExtensions.AddSingleton<IIUC285Proxy>(services,
                (Func<IServiceProvider, IIUC285Proxy>)(provider =>
                {
                    if (singletonIUC285Proxy == null)
                    {
                        singletonIUC285Proxy = (IIUC285Proxy)new IUC285Proxy(
                            ServiceProviderServiceExtensions.GetService<IRBA_API>(provider),
                            ServiceProviderServiceExtensions.GetService<ILogger<IUC285Proxy>>(provider),
                            (IIUC285Notifier)null, (IKioskDataServiceClient)null, (IApplicationSettings)null,
                            ServiceProviderServiceExtensions
                                .GetService<IOptionsMonitor<DeviceServiceConfig>>(provider));
                        Log.Information("Begin Connect");
                        if (singletonIUC285Proxy is IUC285Proxy iuC285Proxy2) iuC285Proxy2.Connect();
                    }

                    return singletonIUC285Proxy;
                }));
            ServiceCollectionServiceExtensions.AddSingleton<IFileUpdater>(services,
                (Func<IServiceProvider, IFileUpdater>)(provider => singletonIUC285Proxy as IFileUpdater));
            return ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
        }

        private static void Main(string[] args)
        {
            Logger logger;
            Log.Logger = logger = ConsoleLoggerConfigurationExtensions.Console(
                ThreadLoggerConfigurationExtensions
                    .WithThreadId(ConfigurationLoggerConfigurationExtensions
                        .Configuration(new LoggerConfiguration().ReadFrom, GetConfiguration(), (DependencyContext)null)
                        .Enrich).Enrich.FromLogContext().WriteTo, (LogEventLevel)0,
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", (IFormatProvider)null,
                (LoggingLevelSwitch)null, new LogEventLevel?(), (ConsoleTheme)null).CreateLogger();
            logger.Information("-------------- Begin RBA Installer --------------");
            logger.Information($"Version: {DeviceService.Domain.DeviceService.AssemblyVersion}");
            try
            {
                HostingHostBuilderExtensions.RunConsoleAsync(CreateHostBuilder(args), new CancellationToken());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occurred.");
            }
            finally
            {
                Log.Information("-------------- End RBA Installer --------------");
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return HostingHostBuilderExtensions.ConfigureLogging(new HostBuilder()
                    .ConfigureAppConfiguration(
                        (Action<HostBuilderContext, IConfigurationBuilder>)((builderContext, config) =>
                            JsonConfigurationExtensions.AddJsonFile(config, "Data\\DeviceServiceConfig.json", true,
                                false)))
                    .ConfigureServices((Action<HostBuilderContext, IServiceCollection>)((hostContext, services) =>
                    {
                        RegisterServices(services);
                        ServiceCollectionHostedServiceExtensions.AddHostedService<RBAInstaller>(services);
                    })),
                (Action<HostBuilderContext, ILoggingBuilder>)((hostContext, logging) =>
                    SerilogLoggingBuilderExtensions.AddSerilog(logging, null, false)));
        }
    }
}