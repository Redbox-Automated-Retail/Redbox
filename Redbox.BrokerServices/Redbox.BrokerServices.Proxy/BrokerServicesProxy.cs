using Belikov.GenuineChannels;
using Belikov.GenuineChannels.DotNetRemotingLayer;
using Belikov.GenuineChannels.GenuineTcp;
using Belikov.GenuineChannels.TransportContext;
using Redbox.Core;
using Redbox.KioskEngine.ComponentModel;
using Redbox.Rental.Model;
using Redbox.Services.KioskBrokerServices.KioskShared.DomainObjects;
using Redbox.Services.KioskBrokerServices.KioskShared.ServiceInterfaces;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace Redbox.BrokerServices.Proxy
{
  public class BrokerServicesProxy : MarshalByRefObject, IKioskOperations
  {
    private const int DefaultBrokerServicesTimeout = 30000;
    private const string DefaultBrokerServicesUrl = "gtcp://blt.accessredbox.net:7890/RegistrationBroker/Default.rem";
    private int? m_kioskId;
    private string m_versionPricingDictionary;
    private bool m_allowReconnect;
    private Timer m_reconnectTimer;
    private IKioskRegister m_broker;
    private GenuineTcpChannel m_channel;
    private volatile bool m_isConnecting;

    public static BrokerServicesProxy Instance => Singleton<BrokerServicesProxy>.Instance;

    public override object InitializeLifetimeService() => (object) null;

    public void Ping() => LogHelper.Instance.Log("BrokerServicesProxy received a remote ping.");

    public void Reset()
    {
      this.m_allowReconnect = false;
      if (this.m_reconnectTimer != null)
      {
        LogHelper.Instance.Log("Reset called...Stop and dispose reconnect timer.");
        try
        {
          this.m_reconnectTimer.Dispose();
        }
        catch (Exception ex)
        {
          LogHelper.Instance.Log("An unhandled exception was raised while disposing the broker services reconnect timer during Reset().", ex);
        }
        finally
        {
          this.m_reconnectTimer = (Timer) null;
        }
      }
      this.DisconnectFromBroker();
    }

    public bool Register(int kioskId, string versionPricingDictionary)
    {
      IConfiguration service = ServiceLocator.Instance.GetService<IConfiguration>();
      if ((service != null ? (service.UseLegacyBrokerService ? 1 : 0) : 0) != 0)
      {
        this.m_kioskId = new int?(kioskId);
        this.m_versionPricingDictionary = versionPricingDictionary;
        this.m_allowReconnect = true;
        return this.ConnectToBroker();
      }
      LogHelper.Instance.Log("BrokerServiceProxy.Register aborted.  Config setting UseLegacyBrokerService not enabled.");
      return false;
    }

    public ReservationResult Reserve(ReservationCart cart) => (ReservationResult) null;

    public ReservationResult Reserve2(ReservationCart2 cart) => (ReservationResult) null;

    public ReservationResult Reserve3(ReservationCart3 cart)
    {
      ReservationCallbackEntry3 reservationCallbackEntry3 = new ReservationCallbackEntry3()
      {
        Cart = cart
      };
      try
      {
        using (ExecutionTimer executionTimer = new ExecutionTimer())
        {
          ICallbackService service = ServiceLocator.Instance.GetService<ICallbackService>();
          if (service != null)
          {
            service.EnqueueCallback((ICallbackEntry) reservationCallbackEntry3);
            reservationCallbackEntry3.ResetEvent.WaitOne();
            LogHelper.Instance.Log("BrokerServicesProxy.Reserve3 execution time: {0}", (object) executionTimer.Elapsed);
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Instance.Log("An unhandled exception was raised in BrokerServicesProxy.Reserve3.", ex);
        reservationCallbackEntry3.BrokerResult.ErrorMessage = "An error ocurred during reserve3.";
        reservationCallbackEntry3.ResetEvent.Set();
      }
      finally
      {
        reservationCallbackEntry3.ResetEvent.Close();
      }
      return reservationCallbackEntry3.BrokerResult;
    }

    public CancelReservationResult CancelReservation(long referenceNumber)
    {
      CancelReservationCallbackEntry reservationCallbackEntry = new CancelReservationCallbackEntry(referenceNumber);
      try
      {
        using (ExecutionTimer executionTimer = new ExecutionTimer())
        {
          ICallbackService service = ServiceLocator.Instance.GetService<ICallbackService>();
          if (service != null)
          {
            service.EnqueueCallback((ICallbackEntry) reservationCallbackEntry);
            reservationCallbackEntry.ResetEvent.WaitOne();
            LogHelper.Instance.Log("BrokerServicesProxy.CancelReservation execution time: {0}", (object) executionTimer.Elapsed);
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Instance.Log("An unhandled exception was raised in BrokerServicesProxy.CancelReservation.", ex);
        reservationCallbackEntry.BrokerResult.ErrorMessage = "An error ocurred during CancelReservation.";
        reservationCallbackEntry.ResetEvent.Set();
      }
      finally
      {
        reservationCallbackEntry.ResetEvent.Close();
      }
      return reservationCallbackEntry.BrokerResult;
    }

    public int Timeout
    {
      get
      {
        return ServiceLocator.Instance.GetService<IMachineSettingsStore>().GetValue<int>("Remote Services\\Reservation Services", "BrokerServicesTimeout", 30000);
      }
      set
      {
        ServiceLocator.Instance.GetService<IMachineSettingsStore>().SetValue<int>("Remote Services\\Reservation Services", "BrokerServicesTimeout", value);
        this.RegisterChannel();
        if (!this.m_kioskId.HasValue || string.IsNullOrEmpty(this.m_versionPricingDictionary))
          return;
        this.ConnectToBroker();
      }
    }

    public string Url
    {
      get
      {
        return ServiceLocator.Instance.GetService<IMachineSettingsStore>().GetValue<string>("Remote Services\\Reservation Services", "BrokerServicesURL", "gtcp://blt.accessredbox.net:7890/RegistrationBroker/Default.rem");
      }
      set
      {
        ServiceLocator.Instance.GetService<IMachineSettingsStore>().SetValue<string>("Remote Services\\Reservation Services", "BrokerServicesURL", value);
      }
    }

    private BrokerServicesProxy()
    {
      GenuineGlobalEventProvider.GenuineChannelsGlobalEvent += (GenuineChannelsGlobalEventHandler) ((sender, args) =>
      {
        try
        {
          if (args.SourceException != null)
            LogHelper.Instance.Log("An unhandled exception has been raised by GenuineChannels.", args.SourceException);
          LogHelper.Instance.Log("GenuineChannels event raised, EventType: {0}", (object) args.EventType);
          switch (args.EventType)
          {
            case GenuineEventType.GeneralServerRestartDetected:
            case GenuineEventType.GeneralConnectionClosed:
            case GenuineEventType.HostResourcesReleased:
              if (!this.m_allowReconnect || this.m_reconnectTimer != null)
                break;
              LogHelper.Instance.Log("...Start reconnect timer, interval 60 seconds.");
              this.m_reconnectTimer = new Timer((TimerCallback) (o => this.ConnectToBroker()), (object) null, 1000, 60000);
              break;
          }
        }
        catch (Exception ex)
        {
          LogHelper.Instance.Log("An unhandled exception was raised in GenuineGlobalEventProvider.GenuineChannelsGlobalEvent.", ex);
        }
      });
      this.RegisterChannel();
    }

    private bool ConnectToBroker()
    {
      if (this.m_isConnecting)
        return false;
      bool broker = false;
      try
      {
        this.m_isConnecting = true;
        this.DisconnectFromBroker();
        this.m_broker = (IKioskRegister) Activator.GetObject(typeof (IKioskRegister), this.Url);
        if (this.m_broker == null)
          return false;
        LogHelper.Instance.Log("Register with Broker Services, store number: {0}, version data: {1}", (object) this.m_kioskId, (object) this.m_versionPricingDictionary);
        this.m_broker.Register(this.m_kioskId.Value, (IKioskOperations) this, this.m_versionPricingDictionary);
        LogHelper.Instance.Log("...Registration with Broker Services successful.");
        broker = true;
      }
      catch (Exception ex)
      {
        LogHelper.Instance.Log("An unhandled exception was raised in BrokerServicesProxy.ConnectToBroker.", ex);
      }
      finally
      {
        this.m_isConnecting = false;
        if (broker && this.m_reconnectTimer != null)
        {
          LogHelper.Instance.Log("...Stop and dispose reconnect timer.");
          try
          {
            this.m_reconnectTimer.Dispose();
          }
          catch (Exception ex)
          {
            LogHelper.Instance.Log("An unhandled exception was raised while disposing the broker services reconnect timer during ConnectToBroker().", ex);
          }
          finally
          {
            this.m_reconnectTimer = (Timer) null;
          }
        }
      }
      return broker;
    }

    private void DisconnectFromBroker()
    {
      try
      {
        if (this.m_broker == null)
          return;
        HostInformation hostInformation = GenuineUtility.FetchHostInformationFromMbr((MarshalByRefObject) this.m_broker);
        if (hostInformation == null)
          return;
        GenuineUtility.FetchTransportContextFromMbr((MarshalByRefObject) this.m_broker).KnownHosts.ReleaseHostResources(hostInformation, (Exception) new ApplicationException("Manual client disconnection."));
        this.m_broker = (IKioskRegister) null;
      }
      catch (Exception ex)
      {
        LogHelper.Instance.Log("An unhandled exception was raised in BrokerServicesProxy.DisconnectFromBroker.", ex);
      }
    }

    private void RegisterChannel()
    {
      try
      {
        if (this.m_channel != null)
          this.UnregisterChannel();
        this.m_channel = new GenuineTcpChannel((IDictionary) new ListDictionary()
        {
          {
            (object) "ConnectTimeout",
            (object) 5000
          },
          {
            (object) "MaxTotalSize",
            (object) 500000
          },
          {
            (object) "MaxContentSize",
            (object) 500000
          },
          {
            (object) "MaxQueuedItems",
            (object) 500000
          },
          {
            (object) "ReconnectionTries",
            (object) 1
          },
          {
            (object) "InvocationTimeout",
            (object) this.Timeout
          },
          {
            (object) "MaxTimeSpanToReconnect",
            (object) 30000
          },
          {
            (object) "SleepBetweenReconnections",
            (object) 5000
          },
          {
            (object) "PersistentConnectionSendPingAfterInactivity",
            (object) 20000
          }
        }, (IClientChannelSinkProvider) new BinaryClientFormatterSinkProvider(), (IServerChannelSinkProvider) new BinaryServerFormatterSinkProvider());
        ChannelServices.RegisterChannel((IChannel) this.m_channel, false);
      }
      catch (Exception ex)
      {
        LogHelper.Instance.Log("An unhandled exception was raised in BrokerServicesProxy.RegisterChannel.", ex);
      }
    }

    private void UnregisterChannel()
    {
      try
      {
        ChannelServices.UnregisterChannel((IChannel) this.m_channel);
        this.m_channel = (GenuineTcpChannel) null;
      }
      catch (Exception ex)
      {
        LogHelper.Instance.Log("An unhandled exception was rasied in BrokerServicesProxy.UnregisterChannel.", ex);
      }
    }
  }
}
