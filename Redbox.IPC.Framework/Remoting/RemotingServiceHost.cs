using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Redbox.Core;

namespace Redbox.IPC.Framework.Remoting
{
    public class RemotingServiceHost : IPCServiceHost
    {
        private TcpServerChannel m_tcpChannel;

        public RemotingServiceHost(int maxThreads)
        {
            MaxThreads = maxThreads;
        }

        public int MaxThreads { get; set; }

        public override bool Start()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["CallRemotingConfigure"]))
            {
                LogHelper.Instance.Log("Before configure - CustomErrors enabled = {0}, {1}",
                    RemotingConfiguration.CustomErrorsEnabled(false), RemotingConfiguration.CustomErrorsMode);
                RemotingConfiguration.Configure(null, Protocol.IsSecure);
                LogHelper.Instance.Log("After configure - CustomErrors enabled = {0}, {1}",
                    RemotingConfiguration.CustomErrorsEnabled(false), RemotingConfiguration.CustomErrorsMode);
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomErrorsMode"]))
                RemotingConfiguration.CustomErrorsMode =
                    Enum<CustomErrorsModes>.ParseIgnoringCase(ConfigurationManager.AppSettings["CustomErrorsMode"],
                        CustomErrorsModes.RemoteOnly);
            LogHelper.Instance.Log("CustomErrors enabled = {0}, {1}", RemotingConfiguration.CustomErrorsEnabled(false),
                RemotingConfiguration.CustomErrorsMode);
            var properties = new ListDictionary
            {
                {
                    "port",
                    Protocol.Port
                },
                {
                    "name",
                    "ipc_tcp"
                }
            };
            LogHelper.Instance.Log("Creating tcp channel for remoting");
            m_tcpChannel = new TcpServerChannel(properties,
                new RemotingServerChannelSinkProvider(this, new BinaryServerFormatterSinkProvider()));
            LogHelper.Instance.Log("Registering channel with channel services");
            ChannelServices.RegisterChannel(m_tcpChannel, Protocol.IsSecure);
            var entry = new WellKnownServiceTypeEntry(typeof(RemotingHost), "CommandService.rem",
                WellKnownObjectMode.SingleCall);
            RemotingConfiguration.ApplicationName = "IPCFramework";
            RemotingConfiguration.RegisterWellKnownServiceType(entry);
            Alive = true;
            Statistics.Instance.ServerStartTime = DateTime.Now;
            LogHelper.Instance.Log("Begin processing incoming requests.");
            return true;
        }

        public override void Stop()
        {
            if (!Alive)
                return;
            m_tcpChannel.StopListening(null);
            ChannelServices.UnregisterChannel(m_tcpChannel);
            Alive = false;
        }
    }
}