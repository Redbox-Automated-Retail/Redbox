using System;
using System.Net;
using System.Net.Sockets;
using Redbox.HAL.Component.Model;

namespace Redbox.HAL.IPC.Framework
{
    public class TcpServiceHost : IPCServiceHost
    {
        private TcpListener m_listener;
        private int m_listenerPort;

        internal TcpServiceHost(IIpcProtocol protocol, IHostInfo info)
            : base(protocol, info)
        {
        }

        protected override void OnStart()
        {
            Alive = true;
            IPAddress address1;
            IPAddress address2;
            if (IPAddress.TryParse(Protocol.Host, out address1))
            {
                address2 = address1;
            }
            else
            {
                address2 = IPAddressHelper.GetAddressForHostName(Protocol.Host);
                if (address2 == null)
                {
                    LogHelper.Instance.Log(
                        string.Format(
                            "Unable to bind to host named '{0}' on port {1},  ensure there is a valid network interface associated to this host and that the designated port is available.",
                            Protocol.Host, Protocol.Port), LogEntryType.Fatal);
                    return;
                }
            }

            if (!int.TryParse(Protocol.Port, out m_listenerPort))
            {
                LogHelper.Instance.Log(
                    string.Format("Unable to create a listener on port '{0}'; maybe your uri is wrong?", Protocol.Port),
                    LogEntryType.Fatal);
            }
            else
            {
                m_listener = new TcpListener(new IPEndPoint(address2, m_listenerPort));
                m_listener.Start();
                LogHelper.Instance.Log("TCP Server: start processing incoming requests on address {0}.", address2);
                Statistics.Instance.ServerStartTime = DateTime.Now;
                while (Alive)
                    try
                    {
                        var tcpServerSession = new TcpServerSession(m_listener.AcceptTcpClient(), this);
                        Register(tcpServerSession);
                        tcpServerSession.Start();
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode != SocketError.Interrupted)
                            LogHelper.Instance.Log("An unhandled socket exception occurred", ex);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.Log("An unhandled exception was raised.", ex);
                    }

                LogHelper.Instance.Log("End processing incoming requests.");
            }
        }

        protected override void OnStop()
        {
            Alive = false;
            m_listener.Stop();
        }
    }
}