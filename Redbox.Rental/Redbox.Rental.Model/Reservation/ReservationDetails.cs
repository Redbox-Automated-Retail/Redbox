using Redbox.Rental.Model.Pricing;
using System;
using System.Collections.Generic;

namespace Redbox.Rental.Model.Reservation
{
    public class ReservationDetails : IReservationDetails
    {
        private List<IReservedTitle> _reservedTitles = new List<IReservedTitle>();
        private List<int> _historyTitlesIds = new List<int>();
        private List<string> _creditCardIds = new List<string>();
        private List<string> _giftCardIds = new List<string>();

        public string ReferenceNumber { get; set; }

        public bool IsMultiNightPrice { get; set; }

        public decimal TaxRate { get; set; }

        public bool AppliedHiveOnlinePromo { get; set; }

        public decimal SubTotal { get; set; }

        public DateTime TransactionDate { get; set; }

        public string PromoCode { get; set; }

        public bool IsMultiDiscVend { get; set; }

        public string ZipCode { get; set; }

        public bool IsGiftCard { get; set; }

        public bool AuthorizeAtPickup { get; set; }

        public string CustomerNumber { get; set; }

        public decimal GrandTotal { get; set; }

        public List<IReservedTitle> ReservedTitles
        {
            get => _reservedTitles;
            set => _reservedTitles = value;
        }

        public decimal Tax { get; set; }

        public decimal HiveOnlinePromoDiscount { get; set; }

        public string EmailAddress { get; set; }

        public int ReservationVersion { get; set; }

        public List<int> HistoryTitleIds
        {
            get => _historyTitlesIds;
            set => _historyTitlesIds = value;
        }

        public List<string> CreditCardIds
        {
            get => _creditCardIds;
            set => _creditCardIds = value;
        }

        public decimal DiscountedSubTotal { get; set; }

        public bool IsLoyaltyRedemption { get; set; }

        public bool AppliedTitleMarketing { get; set; }

        public List<string> GiftCardIds
        {
            get => _giftCardIds;
            set => _giftCardIds = value;
        }

        public decimal DefaultServiceFee { get; set; }

        public decimal ActualServiceFee { get; set; }

        public IPricingSet PricingSet { get; set; }
    }
}