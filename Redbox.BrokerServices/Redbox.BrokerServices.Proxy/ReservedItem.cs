using Redbox.BrokerServices.Proxy.ComponentModel;

namespace Redbox.BrokerServices.Proxy
{
  public class ReservedItem : IReservedItem
  {
    public string Barcode { get; set; }

    public int ProductId { get; set; }
  }
}
