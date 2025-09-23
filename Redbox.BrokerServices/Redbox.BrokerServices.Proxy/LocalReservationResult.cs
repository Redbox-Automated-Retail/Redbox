using Redbox.BrokerServices.Proxy.ComponentModel;
using System.Collections.Generic;

namespace Redbox.BrokerServices.Proxy
{
  public class LocalReservationResult : IReservationResult
  {
    public LocalReservationResult() => this.Items = new List<IReservedItem>();

    public bool Success { get; set; }

    public string ErrorMessage { get; set; }

    public List<IReservedItem> Items { get; set; }
  }
}
