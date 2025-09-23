using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Redbox.Core;
using Redbox.KioskEngine.ComponentModel;
using Redbox.KioskEngine.Environment;
using Redbox.KioskEngine.Kernel;

namespace Redbox.KioskEngine.Bootstrap
{
	internal static class CommandHelper
	{
		private static readonly IDictionary<string, EngineCommandDelegate> m_commands = new Dictionary<string, EngineCommandDelegate>();

		public static void Initialize()
		{
			LogHelper.Instance.Log("Initialize Engine Command Helper:");
			m_commands.Clear();
			MethodInfo[] methods = typeof(CommandHelper).GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in methods)
			{
				EngineCommandAttribute engineCommandAttribute = (EngineCommandAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(EngineCommandAttribute));
				if (engineCommandAttribute != null)
				{
					LogHelper.Instance.Log("...Register command '{0}' to method: {1}", engineCommandAttribute.Name, methodInfo.ToString());
					m_commands[engineCommandAttribute.Name] = (EngineCommandDelegate)Delegate.CreateDelegate(typeof(EngineCommandDelegate), methodInfo);
				}
			}
		}

		public static void ProcessCommandMessage(string command)
		{
			if (string.IsNullOrEmpty(command))
			{
				LogHelper.Instance.Log("WM_COPYDATA: Command string is null; bailing.");
				return;
			}
			int num = command.IndexOf(":");
			if (num == -1)
			{
				ExecuteCommand(command, new string[0]);
				return;
			}
			string name = command.Substring(0, num);
			string[] parms = command.Substring(num + 1).Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			ExecuteCommand(name, parms);
		}

		[EngineCommand(Name = "ShutdownEngine")]
		internal static void ShutdownEngine(string[] parms)
		{
			try
			{
				LogHelper.Instance.Log("WM_COPYDATA: ShutdownEngine");
				if (EnvironmentFunctions.CanShutdown())
				{
					Application.Exit();
				}
				else
				{
					EnvironmentFunctions.FlagForSafeShutdown();
				}
			}
			catch (Exception e)
			{
				LogHelper.Instance.Log("Error in EngineCommand ShutDownEngine.", e);
			}
		}

		[EngineCommand(Name = "RestartBundle")]
		internal static void RestartBundle(string[] parms)
		{
			try
			{
				LogHelper.Instance.Log("WM_COPYDATA: RestartBundle.");
				if (EnvironmentFunctions.CanRestart())
				{
					LogHelper.Instance.Log("Restarting Bundle...");
					ResourceBundleService.Instance.Restart();
				}
				else
				{
					LogHelper.Instance.Log("CanRestart() Returned False; Not Restarting Bundle.");
				}
			}
			catch (Exception e)
			{
				LogHelper.Instance.Log("Error in EngineCommand RestartBundle.", e);
			}
		}

		[EngineCommand(Name = "BringToFront")]
		internal static void BringToFront(string[] parms)
		{
			LogHelper.Instance.Log("WM_COPYDATA: BringToFront");
			Application.OpenForms[0].Activate();
		}

		[EngineCommand(Name = "ExecuteScript")]
		internal static void ExecuteScript(string[] parms)
		{
			IKernelService service = ServiceLocator.Instance.GetService<IKernelService>();
			if (service == null)
			{
				return;
			}
			try
			{
				foreach (string text in parms)
				{
					LogHelper.Instance.Log("WM_COPYDATA: Execute script: {0}", text);
					service.ExecuteChunk(File.ReadAllText(text));
				}
			}
			catch (Exception e)
			{
				LogHelper.Instance.Log("An unhandled exception was raised in ProcessCommandMessage.ExecuteScript.", e);
			}
		}

		[EngineCommand(Name = "Activate")]
		internal static void Activate(string[] parms)
		{
			try
			{
				string parmsvalue = string.Empty;
				parms.ForEach(delegate(string s)
				{
					parmsvalue += $"{s}|";
				});
				LogHelper.Instance.Log("WM_COPYDATA: Activate bundle: {0}", parmsvalue);
				string[] array = parms[0].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 2)
				{
					ResourceBundleService.Instance.Activate(array[0], array[1]);
				}
			}
			catch (Exception e)
			{
				LogHelper.Instance.Log("An unhandled exception was raised in ProcessCommandMessage.Activate.", e);
			}
		}

		[EngineCommand(Name = "ActivateDefaultBundle")]
		internal static void ActivateDefaultBundle(string[] parms)
		{
			try
			{
				ResourceBundleService.Instance.ActivateDefaultBundle();
			}
			catch (Exception e)
			{
				LogHelper.Instance.Log("An unhandled exception was raised in ProcessCommandMessage.ActivateDefaultBundle.", e);
			}
		}

		[EngineCommand(Name = "ExportQueue")]
		internal static void ExportQueue(string[] parms)
		{
			try
			{
				LogHelper.Instance.Log("WM_COPYDATA: Export queue to file: {0}", parms[0]);
				ServiceLocator.Instance.GetService<IQueueService>()?.ExportToXml(parms[0]);
			}
			catch (Exception e)
			{
				LogHelper.Instance.Log("An unhandled exception was raised in ProcessCommandMessage.ExportQueue.", e);
			}
		}

		private static void ExecuteCommand(string name, string[] parms)
		{
			if (!m_commands.ContainsKey(name))
			{
				LogHelper.Instance.Log("WM_COPYDATA: Command '{0}' does not have a registered handler method.", name);
				return;
			}
			LogHelper.Instance.Log("WM_COPYDATA: Execute command '{0}', with parms: {1}.", name, (parms != null) ? parms.Join("|") : "null");
			m_commands[name](parms);
		}
	}
}
