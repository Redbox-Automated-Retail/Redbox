using System;
using System.Collections.Generic;

namespace Redbox.KioskEngine.ComponentModel.KioskServices
{
    public class CustomerOfferResult
    {
        public long OfferId { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string PromotionCode { get; set; }

        public ICollection<long> IncludeTitleIds { get; set; }

        public ICollection<long> ExcludeTitleIds { get; set; }

        public TimeSpan? ExcludeIfWithinRedboxReleaseDate { get; set; }

        public decimal DiscountAmount { get; set; }

        public string DiscountType { get; set; }

        public string ProductType { get; set; }

        public ICollection<int> IncludeFormatIds { get; set; }

        public ICollection<int> ExcludeFormatIds { get; set; }

        public string OfferScopeType { get; set; }

        public string CustomerSegmentType { get; set; }

        public long? StoredOfferId { get; set; }

        public ICollection<OfferProductResult> OfferProducts { get; set; }

        public string OfferDiscountCode { get; set; }

        public long CampaignId { get; set; }

        public DateTime? RedeemedDateTime { get; set; }

        public string ActionCode { get; set; }
    }
}