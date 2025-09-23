using System;
using System.Reflection;
using NetFwTypeLib;
using Redbox.Core;

namespace Redbox.KioskEngine.Bootstrap
{
	public static class FirewallHelper
	{
		public const string ApplicationName = "Redbox Engine for Distributed Stores";

		private const string OpenPortProgramID = "HNetCfg.FwOpenPort";

		private const string FirewallManagerProgramID = "HNetCfg.FwMgr";

		private const string AuthorizedApplicationProgramID = "HNetCfg.FwAuthorizedApplication";

		private static INetFwMgr m_firewallManager;

		private static INetFwMgr FirewallManager
		{
			get
			{
				if (m_firewallManager == null)
				{
					m_firewallManager = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
				}
				return m_firewallManager;
			}
		}

		public static void CreateFirewallExemptions()
		{
			try
			{
				LogHelper.Instance.Log("Create Windows firewall exemptions:");
				OperatingSystem oSVersion = System.Environment.OSVersion;
				if (oSVersion.Platform != PlatformID.Win32NT || oSVersion.Version.Major != 5 || oSVersion.Version.Minor != 1)
				{
					LogHelper.Instance.Log("...Not running on Windows XP; skipping firewall exemptions.");
				}
				else if (IsApplicationAuthorized("Redbox Engine for Distributed Stores"))
				{
					LogHelper.Instance.Log("...Application '{0}' is already exempt.", "Redbox Engine for Distributed Stores");
				}
				else
				{
					AuthorizeApplication("Redbox Engine for Distributed Stores", Assembly.GetExecutingAssembly().Location, NET_FW_SCOPE_.NET_FW_SCOPE_ALL, NET_FW_IP_VERSION_.NET_FW_IP_VERSION_V4);
					LogHelper.Instance.Log("...Exemption created for application named: '{0}'.", "Redbox Engine for Distributed Stores");
				}
			}
			catch (Exception e)
			{
				LogHelper.Instance.Log("An unhandled exception was rasied in the FirewallHelper.CreateFirewallExemptions", e);
			}
		}

		internal static void AddPortExemption(int port, NET_FW_IP_PROTOCOL_ protocol, string portName)
		{
			INetFwOpenPort netFwOpenPort = (INetFwOpenPort)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwOpenPort"));
			netFwOpenPort.Port = port;
			netFwOpenPort.Name = portName;
			netFwOpenPort.Enabled = true;
			netFwOpenPort.Protocol = protocol;
			netFwOpenPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
			FirewallManager.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(netFwOpenPort);
		}

		internal static void RemovePortExemption(int port, NET_FW_IP_PROTOCOL_ protocol)
		{
			FirewallManager.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, protocol);
		}

		internal static void AuthorizeApplication(string title, string applicationPath, NET_FW_SCOPE_ scope, NET_FW_IP_VERSION_ ipVersion)
		{
			INetFwAuthorizedApplication netFwAuthorizedApplication = (INetFwAuthorizedApplication)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication"));
			netFwAuthorizedApplication.Name = title;
			netFwAuthorizedApplication.Scope = scope;
			netFwAuthorizedApplication.Enabled = true;
			try
			{
				netFwAuthorizedApplication.IpVersion = ipVersion;
			}
			catch (Exception e)
			{
				LogHelper.Instance.Log("An unhandled exception was raised in FirewallHelper.AuthorizeApplication.", e);
			}
			netFwAuthorizedApplication.ProcessImageFileName = applicationPath;
			FirewallManager.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(netFwAuthorizedApplication);
		}

		internal static bool IsApplicationAuthorized(string name)
		{
			foreach (INetFwAuthorizedApplication authorizedApplication in FirewallManager.LocalPolicy.CurrentProfile.AuthorizedApplications)
			{
				if (authorizedApplication.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		internal static void RemoveAuthorizedApplication(string applicationPath)
		{
			FirewallManager.LocalPolicy.CurrentProfile.AuthorizedApplications.Remove(applicationPath);
		}

		internal static int? GetPortNumberAndProtocol(string protocolType, int port, out NET_FW_IP_PROTOCOL_ protocol)
		{
			protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
			if (!string.IsNullOrEmpty(protocolType) && string.Compare(protocolType, "UDP") == 0)
			{
				protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
			}
			return port;
		}
	}
}
