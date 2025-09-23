using Redbox.BrokerServices.Proxy.ComponentModel;
using Redbox.Core;
using Redbox.KioskEngine.ComponentModel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Redbox.BrokerServices.Proxy
{
  public class ReservationServicesProxy : IReservationServices
  {
    private int m_working;

    public static ReservationServicesProxy Instance => Singleton<ReservationServicesProxy>.Instance;

    public bool CanSwitch() => this.m_working == 0;

    public void Reset() => BrokerServicesProxy.Instance.Reset();

    public void Register(
      string storeNumber,
      IDictionary<string, IDictionary<string, IDictionary<string, Decimal>>> prices,
      ReservationRequestCallback3 reservationRequestCallback,
      CancelReservationCallback cancelRequestCallback)
    {
      Dictionary<string, object> instance = new Dictionary<string, object>();
      instance["Runtime"] = (object) "Kiosk Engine";
      instance["Version"] = (object) AssemblyInfoHelper.GetVersion(Assembly.GetExecutingAssembly());
      instance["PricingPolicy"] = (object) prices;
      instance["GiftCardsEnabled"] = (object) false;
      instance["RecoOnPickup"] = (object) false;
      IConfigurationService service = ServiceLocator.Instance.GetService<IConfigurationService>();
      if (service != null)
      {
        bool flag1;
        if (service.TryGetValue<bool>("system", "General", "GiftCardsEnabled", out flag1))
          instance["GiftCardsEnabled"] = (object) flag1;
        bool flag2;
        if (service.TryGetValue<bool>("system", "General", "RecommendationOnPickup", out flag2))
          instance["RecoOnPickup"] = (object) flag2;
      }
      instance["AuthorizeAtPickup"] = (object) Redbox.Rental.Services.Configuration.Configuration.Instance.AuthorizeAtPickup;
      instance["CancelReservation"] = (object) true;
      instance["Features"] = (object) Features.MultiDiscVend;
      int result;
      if (!int.TryParse(storeNumber, out result))
        return;
      this.RequestCallback3 = reservationRequestCallback;
      this.CancelRequestCallback = cancelRequestCallback;
      BrokerServicesProxy.Instance.Register(result, instance.ToJson());
    }

    public IReservedItem NewReservedItem() => (IReservedItem) new ReservedItem();

    public IReservationResult NewReservationResult()
    {
      return (IReservationResult) new LocalReservationResult();
    }

    public ILocalCancelReservationResult NewCancelReservationResult()
    {
      return (ILocalCancelReservationResult) new LocalCancelReservationResult();
    }

    internal ReservationRequestCallback3 RequestCallback3 { get; private set; }

    internal CancelReservationCallback CancelRequestCallback { get; private set; }

    private ReservationServicesProxy()
    {
    }
  }
}
