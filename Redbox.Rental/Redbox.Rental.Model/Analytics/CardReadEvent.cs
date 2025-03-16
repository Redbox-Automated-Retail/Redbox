using Redbox.KioskEngine.ComponentModel.TrackData;

namespace Redbox.Rental.Model.Analytics
{
    public class CardReadEvent : AnalyticsEvent
    {
        public CardReadEvent(ITrackData trackData)
        {
            EventType = "CardRead";
            CardSourceType = trackData?.CardSourceType.ToString();
            WalletType = trackData?.WalletType.ToString();
            NextCardReadIsInTechnicalFallback = trackData?.NextCardReadIsInTechnicalFallback;
            IsInTechnicalFallback = trackData?.IsInTechnicalFallback;
            CardHasChip = trackData?.CardHasChip;
            FallbackStatusAction = trackData?.FallbackStatusAction.ToString();
            HasVas = trackData != null && trackData.HasVas;
            HasName = new bool?(!string.IsNullOrWhiteSpace(trackData?.FirstName) ||
                                !string.IsNullOrWhiteSpace(trackData?.LastName));
        }

        public bool CardReadValid { get; set; }

        public string Action { get; set; }

        public bool? HasName { get; set; }

        public string CardSourceType { get; set; }

        public string WalletType { get; set; }

        public string FallbackStatusAction { get; set; }

        public bool? IsInTechnicalFallback { get; set; }

        public bool? NextCardReadIsInTechnicalFallback { get; set; }

        public bool? CardHasChip { get; set; }

        public bool HasVas { get; set; }
    }
}