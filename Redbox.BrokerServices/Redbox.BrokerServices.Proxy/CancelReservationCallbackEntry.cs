using Redbox.BrokerServices.Proxy.ComponentModel;
using Redbox.Core;
using Redbox.KioskEngine.ComponentModel;
using Redbox.Services.KioskBrokerServices.KioskShared.DomainObjects;
using System.Threading;

namespace Redbox.BrokerServices.Proxy
{
  public class CancelReservationCallbackEntry : ICallbackEntry
  {
    internal readonly AutoResetEvent ResetEvent = new AutoResetEvent(false);
    private long _referenceNumber;

    public string Name
    {
      get => string.Format("CancelReservationCallbackEntry - {0}", (object) this._referenceNumber);
    }

    public CancelReservationCallbackEntry(long referenceNumber)
    {
      this._referenceNumber = referenceNumber;
      this.BrokerResult = new CancelReservationResult();
    }

    public void Invoke()
    {
      LogHelper.Instance.Log("BrokerServicesProxy received a remote cancel reservation request: {0}", (object) this._referenceNumber);
      if (ReservationServicesProxy.Instance.CancelRequestCallback == null)
      {
        this.BrokerResult.ErrorMessage = "No cancel reservation request callback registered.";
        this.BrokerResult.CancellationSucceeded = false;
        this.ResetEvent.Set();
        LogHelper.Instance.Log("CancelReservationCallbackEntry:Invoke() No cancel reservation request callback registered.");
      }
      else
      {
        ILocalCancelReservationResult instance = ReservationServicesProxy.Instance.CancelRequestCallback(this._referenceNumber);
        this.BrokerResult.CancellationSucceeded = instance.Success;
        this.BrokerResult.ErrorMessage = instance.ErrorMessage;
        LogHelper.Instance.Log("BrokerServicesProxy Cancel reservation response: {0}", (object) instance.ToJson());
        this.ResetEvent.Set();
      }
    }

    public CancelReservationResult BrokerResult { get; }
  }
}
