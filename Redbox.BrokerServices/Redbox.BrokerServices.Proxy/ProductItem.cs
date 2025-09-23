using Redbox.BrokerServices.Proxy.ComponentModel;
using Redbox.KioskEngine.ComponentModel;

namespace Redbox.BrokerServices.Proxy
{
  public class ProductItem : IProductItem
  {
    public string Barcode { get; set; }

    public bool MultiDisc { get; set; }

    public LineItemStatus Status { get; set; }
  }
}
