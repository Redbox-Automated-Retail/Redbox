using System;
using System.Windows;
using Redbox.Rental.UI.ControllersLogic;

namespace Redbox.Rental.UI.Models
{
    public class StoredPromoCodeModel : BaseModel<StoredPromoCodeModel>, IPerksOfferListItem
    {
        private static readonly DependencyProperty IsAdaModeProperty = DependencyProperty.Register("IsAdaMode",
            typeof(bool), typeof(StoredPromoCodeModel), new PropertyMetadata(false));

        public static readonly DependencyProperty IsAddedProperty =
            CreateDependencyProperty("IsAdded", TYPES.BOOL, false);

        private char _maskCharacter;

        private string _promoCode;

        public string PromoCode
        {
            get => _promoCode;
            set
            {
                _promoCode = value;
                MaskPromoCode();
            }
        }

        public int CampaignId { get; set; }

        public bool IsRedboxPlusPromo { get; set; }

        public string MaskedPromoCode { get; private set; }

        public string PromoCodeDescription { get; set; }

        public string PromoCodeExpirationText { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string AddedToBagText { get; set; }

        public bool IsAdded
        {
            get { return Dispatcher.Invoke(() => (bool)GetValue(IsAddedProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(IsAddedProperty, value); }); }
        }

        public char MaskCharacter
        {
            get => _maskCharacter;
            set
            {
                _maskCharacter = value;
                MaskPromoCode();
            }
        }

        public DynamicRoutedCommand AddRemoveButtonCommand { get; set; }

        public PromoListLogic Logic { get; set; }

        public int AdaButtonNumber { get; set; }

        public bool IsAdaMode
        {
            get { return Dispatcher.Invoke(() => (bool)GetValue(IsAdaModeProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(IsAdaModeProperty, value); }); }
        }

        public int NumberOfSpaces => 1;

        private void MaskPromoCode()
        {
            if (MaskCharacter == '\0') MaskCharacter = 'X';
            var promoCode = _promoCode;
            if ((promoCode != null ? promoCode.Length : 0) > 4)
            {
                MaskedPromoCode = new string(MaskCharacter, _promoCode.Length - 4) +
                                  _promoCode.Substring(_promoCode.Length - 4);
                return;
            }

            MaskedPromoCode = _promoCode;
        }
    }
}