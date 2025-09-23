using Redbox.BrokerServices.Proxy.ComponentModel;
using Redbox.Core;
using Redbox.KioskEngine.ComponentModel;
using Redbox.Services.KioskBrokerServices.KioskShared.DomainObjects;
using Redbox.Services.KioskBrokerServices.KioskShared.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Redbox.BrokerServices.Proxy
{
  public class ReservationCallbackEntry3 : ICallbackEntry
  {
    internal readonly AutoResetEvent ResetEvent = new AutoResetEvent(false);

    public string Name => nameof (ReservationCallbackEntry3);

    public ReservationCallbackEntry3() => this.BrokerResult = new ReservationResult();

    public void Invoke()
    {
      Dictionary<string, object> instance1 = this.Cart.ToJson().ToObject<Dictionary<string, object>>();
      if (instance1.ContainsKey("Email") && instance1["Email"] != null)
        instance1["Email"] = (object) this.ObfuscateMessageData(instance1["Email"].ToString(), 'x', 1, 0);
      if (instance1.ContainsKey("CardID") && instance1["CardID"] != null)
        instance1["CardID"] = (object) this.ObfuscateMessageData(instance1["CardID"].ToString(), 'x', 3, 3);
      if (instance1.ContainsKey("ReservationCardHash") && instance1["ReservationCardHash"] != null)
      {
        Dictionary<string, object> dictionary = (Dictionary<string, object>) instance1["ReservationCardHash"];
        if (dictionary.ContainsKey("CardBinHash") && dictionary["CardBinHash"] != null)
          dictionary["CardBinHash"] = (object) this.ObfuscateMessageData(dictionary["CardBinHash"].ToString(), 'x', 3, 3);
        if (dictionary.ContainsKey("Last4") && dictionary["Last4"] != null)
          dictionary["Last4"] = (object) this.ObfuscateMessageData(dictionary["Last4"].ToString(), 'x', 1, 0);
      }
      if (instance1.ContainsKey("AlternateCardHashes") && instance1["AlternateCardHashes"] != null)
      {
        foreach (Dictionary<string, object> dictionary in (ArrayList) instance1["AlternateCardHashes"])
        {
          if (dictionary.ContainsKey("CardBinHash") && dictionary["CardBinHash"] != null)
            dictionary["CardBinHash"] = (object) this.ObfuscateMessageData(dictionary["CardBinHash"].ToString(), 'x', 3, 3);
          if (dictionary.ContainsKey("Last4") && dictionary["Last4"] != null)
            dictionary["Last4"] = (object) this.ObfuscateMessageData(dictionary["Last4"].ToString(), 'x', 1, 0);
        }
      }
      LogHelper.Instance.Log("BrokerServicesProxy received a remote reservation request: {0}", (object) instance1.ToJson());
      if (ReservationServicesProxy.Instance.RequestCallback3 == null)
      {
        this.BrokerResult.ErrorMessage = "No reservation request callback registered.";
        this.BrokerResult.ReservationSucceeded = false;
        this.ResetEvent.Set();
        LogHelper.Instance.Log("ReservationCallbackEntry3:Invoke() No reservation request callback registered.");
      }
      else
      {
        Dictionary<string, List<int>> productsToReserve = new Dictionary<string, List<int>>();
        foreach (ReservationTitle3 title in this.Cart.Titles)
        {
          string key = title.ReservationType == ReservationType.Purchase ? "purchase" : "rental";
          List<int> intList;
          if (!productsToReserve.ContainsKey(key))
          {
            intList = new List<int>();
            productsToReserve[key] = intList;
          }
          else
            intList = productsToReserve[key];
          intList.Add(title.TitleID);
        }
        IReservationResult instance2 = ReservationServicesProxy.Instance.RequestCallback3(this.Cart.CardID, this.Cart.Email, this.Cart.ReferenceNumber, this.Cart.ReserveDate, (IDictionary<string, List<int>>) productsToReserve, this.Cart.AppliedCredit, this.Cart.AppliedHiveOnlinePromotion, this.Cart.AppliedTitleMarketing, this.Cart.CustomerNumber, this.Cart.DiscountedSubTotal, this.Cart.GrandTotal, this.Cart.HistoryTitleIDs, this.Cart.HiveOnlinePromotionDiscount, this.Cart.IsMnp, this.Cart.NumberOfCreditsRemaining, this.Cart.PromoCode, this.Cart.SubTotal, this.Cart.Tax, this.Cart.Titles, this.Cart.IsGc, this.Cart.GcIds, this.Cart.CcIds, this.Cart.TaxRate, this.Cart.Zip, this.Cart.IsMdv, this.Cart.LoyaltyRedemption, this.Cart.DefaultServiceFeeAmount, this.Cart.ActualServiceFeeAmount, this.Cart.AuthorizeAtPickup, this.Cart.ReservationCardHash, this.Cart.AlternateCardHashes);
        this.BrokerResult.ReservationSucceeded = instance2.Success;
        this.BrokerResult.ErrorMessage = instance2.ErrorMessage;
        List<TitleResult> titleResultList = new List<TitleResult>();
        foreach (IReservedItem reservedItem in instance2.Items)
        {
          foreach (ReservationTitle3 title in this.Cart.Titles)
          {
            if (title.TitleID == reservedItem.ProductId)
            {
              titleResultList.Add(new TitleResult()
              {
                ItemID = title.ItemID,
                Reservable = reservedItem.Barcode != "empty",
                ReservedBarcode = reservedItem.Barcode
              });
              break;
            }
          }
        }
        this.BrokerResult.TitleResults = titleResultList;
        LogHelper.Instance.Log("BrokerServicesProxy reservation response: {0}", (object) instance2.ToJson());
        this.ResetEvent.Set();
      }
    }

    public ReservationResult BrokerResult { get; private set; }

    public ReservationCart3 Cart { get; internal set; }

    private string ObfuscateMessageData(string msgText, char newChar, int leading, int trailing)
    {
      if (leading + trailing > msgText.Length)
      {
        leading = 0;
        trailing = 0;
      }
      string str1 = leading > 0 ? msgText.Substring(0, leading) : string.Empty;
      string str2 = trailing > 0 ? msgText.Substring(msgText.Length - trailing, trailing) : string.Empty;
      string str3 = new string(newChar, msgText.Length - (leading + trailing));
      string str4 = str2;
      return str1 + str3 + str4;
    }
  }
}
