using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Redbox.Core;
using Redbox.KioskEngine.ComponentModel;
using Redbox.Log.Framework;
using Redbox.Rental.Model;
using Redbox.Rental.Model.Health;

namespace Redbox.KioskEngine.Bootstrap
{
	public class Program : IDisposable
	{
		private class KioskUnhandledException
		{
			public string Message { get; set; }

			public string Exception { get; set; }
		}

		public static Program _program;

		private string _kioskUnhandledExceptionFilePath;

		private static readonly ILogger m_logger = LogHelper.Instance.CreateLog4NetLogger(typeof(Program));

		private static readonly ILogger m_loggerCardReader = LogHelper.Instance.CreateMultiLog4NetLogger("CardReaderLog");

		private static readonly ILogger m_loggerUser = LogHelper.Instance.CreateMultiLog4NetLogger("UserLoginLog");

		private static readonly ILogger m_loggerTestSvcs = LogHelper.Instance.CreateMultiLog4NetLogger("TestServices");

		private static readonly ILogger m_loggerControlPanelAction = LogHelper.Instance.CreateMultiLog4NetLogger("ControlPanelActionLog");

		private static readonly ILogger m_loggerFieldMaintenanceAction = LogHelper.Instance.CreateMultiLog4NetLogger("FieldMaintenanceActionLog");

		private string KioskUnhandledExceptionFilePath
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_kioskUnhandledExceptionFilePath))
				{
					string directoryName = Path.GetDirectoryName(typeof(Program).Assembly.Location);
					_kioskUnhandledExceptionFilePath = Path.Combine(directoryName, "..\\data\\UnhandledException.txt");
				}
				return _kioskUnhandledExceptionFilePath;
			}
		}

		[STAThread]
		public static void Main()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			Application.ThreadException += Application_ThreadException;
			using (_program = new Program())
			{
				_program.Run();
			}
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				Exception ex = (Exception)e.ExceptionObject;
				LogHelper.Instance.Log("Unhandled Application Exception. Exiting Application", ex);
				if (_program != null)
				{
					_program.WriteUnhandledException("Unhandled Application Exception. Exiting Application", ex);
				}
			}
			finally
			{
				Application.Exit();
			}
		}

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			try
			{
				LogHelper.Instance.Log("Unhandled Windows Forms Thread Exception. Exiting Application", e.Exception);
				if (_program != null)
				{
					_program.WriteUnhandledException("Unhandled Windows Forms Thread Exception. Exiting Application", e.Exception);
				}
			}
			finally
			{
				Application.Exit();
			}
		}

		public void Run()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log4net.config");
			m_logger.Configure(path);
			m_loggerCardReader.Configure(path);
			m_loggerUser.Configure(path);
			m_loggerTestSvcs.Configure(path);
			LogHelper.Instance.Loggers.Add(m_logger);
			LogHelper.Instance.Loggers.Add(m_loggerCardReader);
			LogHelper.Instance.Loggers.Add(m_loggerUser);
			LogHelper.Instance.Loggers.Add(m_loggerControlPanelAction);
			LogHelper.Instance.Loggers.Add(m_loggerFieldMaintenanceAction);
			LogHelper.Instance.Loggers.Add(m_loggerTestSvcs);
			LogHelper.Instance.Log("Setting System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12");
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			try
			{
				ErrorList errorList = EngineApplication.Instance.Initialize();
				if (errorList.ContainsCode("INSTANCE"))
				{
					return;
				}
				if (errorList.ContainsError())
				{
					errorList.ToLogHelper();
					ErrorForm errorForm = new ErrorForm();
					errorForm.Errors = errorList;
					errorForm.ShowDialog();
				}
				else
				{
					ReadUnhandledExceptionAndSendAlert();
					if (EngineApplication.Instance.Run())
					{
						Application.Run(EngineApplication.Instance);
					}
				}
			}
			catch (Exception ex)
			{
				LogHelper.Instance.Log("Unhandled exception occured in Program.Run. Exiting Application", ex);
				WriteUnhandledException("Unhandled exception occured in Program.Run. Exiting Application", ex);
				Application.Exit();
			}
		}

		public void WriteUnhandledException(string message, Exception exception)
		{
			try
			{
				LogHelper.Instance.Log("Program.WriteUnhandledException");
				KioskUnhandledException data = new KioskUnhandledException
				{
					Message = message,
					Exception = LogException(string.Empty, exception)
				};
				WriteUnhandledExceptionData(data);
			}
			catch (Exception e)
			{
				LogHelper.Instance.LogException($"Program.WriteUnhandledException - an exception occurred", e);
			}
		}

		private void ReadUnhandledExceptionAndSendAlert()
		{
			try
			{
				LogHelper.Instance.Log("Program.ReadUnhandledExceptionAndSendAlert");
				KioskUnhandledException ex = ReadUnhandledExceptionData();
				if (ex != null)
				{
					LogHelper.Instance.Log("Program.ReadUnhandledExceptionAndSendAlert - Kiosk unhandled exception found, sending alert");
					DeleteUnhandledExceptionData();
					SendUnhandledExceptionAlert(ex);
				}
			}
			catch (Exception e)
			{
				LogHelper.Instance.LogException($"Program.ReadUnhandledExceptionAndSendAlert - an exception occurred", e);
				DeleteUnhandledExceptionData();
			}
		}

		private void SendUnhandledExceptionAlert(KioskUnhandledException unhandledException)
		{
			IHealthServices service = ServiceLocator.Instance.GetService<IHealthServices>();
			if (service == null)
			{
				return;
			}
			IStoreManager service2 = ServiceLocator.Instance.GetService<IStoreManager>();
			if (service2 == null)
			{
				return;
			}
			IConfigurationService service3 = ServiceLocator.Instance.GetService<IConfigurationService>();
			if (service3 != null)
			{
				bool flag = true;
				if (service3.TryGetValue<bool>("system", "General", "SendAppCrashAlert", out var value))
				{
					flag = value;
				}
				LogHelper.Instance.Log("Program.SendUnhandledExceptionAlert - SendAppCrashAlert is {0}", flag);
				if (flag)
				{
					service.SendAlert(service2.KioskId.ToString(), "ApplicationCrash", "Unhandled", string.Format($"Message: {unhandledException.Message}, Exception: {unhandledException.Exception}"), DateTime.Now, null);
				}
			}
		}

		private void WriteUnhandledExceptionData(KioskUnhandledException data)
		{
			try
			{
				string text = data.ToJson();
				byte[] bytes = Encoding.ASCII.GetBytes(text);
				File.WriteAllBytes(KioskUnhandledExceptionFilePath, bytes);
				LogHelper.Instance.Log(text, LogEntryType.Error);
			}
			catch (Exception e)
			{
				LogHelper.Instance.LogException($"Program.WriteUnhandledExceptionData - an exception occurred writing {KioskUnhandledExceptionFilePath}.", e);
			}
		}

		private KioskUnhandledException ReadUnhandledExceptionData()
		{
			try
			{
				if (!File.Exists(KioskUnhandledExceptionFilePath))
				{
					return null;
				}
				byte[] bytes = File.ReadAllBytes(KioskUnhandledExceptionFilePath);
				return Encoding.ASCII.GetString(bytes).ToObject<KioskUnhandledException>();
			}
			catch (Exception e)
			{
				LogHelper.Instance.LogException($"Program.WriteUnhandledExceptionData - an exception occurred reading {KioskUnhandledExceptionFilePath}.", e);
			}
			return null;
		}

		private void DeleteUnhandledExceptionData()
		{
			try
			{
				if (File.Exists(KioskUnhandledExceptionFilePath))
				{
					File.Delete(KioskUnhandledExceptionFilePath);
					LogHelper.Instance.Log("Program.DeleteUnhandledExceptionData - Deleting {0}", KioskUnhandledExceptionFilePath);
				}
			}
			catch (Exception e)
			{
				LogHelper.Instance.LogException($"Program.DeleteUnhandledExceptionData - an exception occurred deleting {KioskUnhandledExceptionFilePath}.", e);
			}
		}

		private string LogException(string prefix, Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"{prefix}Message:{ex.Message}");
			stringBuilder.AppendLine($"{prefix}StackTrace{ex.StackTrace}");
			if (ex.InnerException != null)
			{
				stringBuilder.AppendLine(LogException("inner:", ex.InnerException));
			}
			return stringBuilder.ToString();
		}

		public void Dispose()
		{
		}
	}
}
