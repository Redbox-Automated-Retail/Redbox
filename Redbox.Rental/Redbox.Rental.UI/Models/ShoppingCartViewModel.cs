using System;
using System.Collections.Generic;
using System.Windows;
using Redbox.KioskEngine.ComponentModel.TextToSpeech;

namespace Redbox.Rental.UI.Models
{
    public class ShoppingCartViewModel : DependencyObject
    {
        public static readonly DependencyProperty PayButtonText1Property = DependencyProperty.Register("PayButtonText1",
            typeof(string), typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty PayButtonText2Property = DependencyProperty.Register("PayButtonText2",
            typeof(string), typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty PayButtonText1VisibilityProperty =
            DependencyProperty.Register("PayButtonText1Visibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty AcceptAndPayTextProperty =
            DependencyProperty.Register("AcceptAndPayText", typeof(string), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty PricingModelProperty = DependencyProperty.Register("PricingModel",
            typeof(PricingModel), typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty PromoCodeButtonEnabledProperty =
            DependencyProperty.Register("PromoCodeButtonEnabled", typeof(bool), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty PayButtonEnabledProperty =
            DependencyProperty.Register("PayButtonEnabled", typeof(bool), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty ADASpacerVisibilityProperty =
            DependencyProperty.Register("ADASpacerVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty AddMovieButtonVisibilityProperty =
            DependencyProperty.Register("AddMovieButtonVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty UpdateBagButtonVisibilityProperty =
            DependencyProperty.Register("UpdateBagButtonVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty DisplayProductModelsProperty =
            DependencyProperty.Register("DisplayProductModels", typeof(List<DisplayCheckoutProductModel>),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty SignInIncentiveTextVisibilityProperty =
            DependencyProperty.Register("SignInIncentiveTextVisibility", typeof(Visibility),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty SignInButtonVisibilityProperty =
            DependencyProperty.Register("SignInButtonVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty UsePointsButtonTextProperty =
            DependencyProperty.Register("UsePointsButtonText", typeof(string), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty UsePointsButtonEnabledProperty =
            DependencyProperty.Register("UsePointsButtonEnabled", typeof(bool), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty UsePointsButtonVisibilityProperty =
            DependencyProperty.Register("UsePointsButtonVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty PromoCodeButtonMarginProperty =
            DependencyProperty.Register("PromoCodeButtonMargin", typeof(Thickness), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(new Thickness(0.0, 0.0, 0.0, 74.0)));

        public static readonly DependencyProperty LoyaltySystemErrorTextProperty =
            DependencyProperty.Register("LoyaltySystemErrorText", typeof(string), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty LoyaltySystemErrorTextVisibilityProperty =
            DependencyProperty.Register("LoyaltySystemErrorTextVisibility", typeof(Visibility),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty PointsBalanceTextProperty =
            DependencyProperty.Register("PointsBalanceText", typeof(string), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty PointsBalanceInsufficientMessageTextProperty =
            DependencyProperty.Register("PointsBalanceInsufficientMessageText", typeof(string),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty PointsBalanceSufficientMessageTextProperty =
            DependencyProperty.Register("PointsBalanceSufficientMessageText", typeof(string),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty PointsBalanceInsufficientMessageTextAlignmentProperty =
            DependencyProperty.Register("PointsBalanceInsufficientMessageTextAlignment", typeof(TextAlignment),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(TextAlignment.Left));

        public static readonly DependencyProperty PointsBalanceStackPanelVisibilityProperty =
            DependencyProperty.Register("PointsBalanceStackPanelVisibility", typeof(Visibility),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty PointsBalanceTextVisibilityProperty =
            DependencyProperty.Register("PointsBalanceTextVisibility", typeof(Visibility),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty PointsBalanceInfoButtonVisibilityProperty =
            DependencyProperty.Register("PointsBalanceInfoButtonVisibility", typeof(Visibility),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty PointsBalanceInsufficientMessageTextVisibilityProperty =
            DependencyProperty.Register("PointsBalanceInsufficientMessageTextVisibility", typeof(Visibility),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty PointsBalanceSufficientMessageStackPanelVisibilityProperty =
            DependencyProperty.Register("PointsBalanceSufficientMessageStackPanelVisibility", typeof(Visibility),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty ServiceFeeTextVisibilityProperty =
            DependencyProperty.Register("ServiceFeeTextVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty MDVFootnoteVisibilityProperty =
            DependencyProperty.Register("MDVFootnoteVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty PointsEstimateTextProperty =
            DependencyProperty.Register("PointsEstimateText", typeof(string), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty PointsEstimateVisibilityProperty =
            DependencyProperty.Register("PointsEstimateVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty RedboxPlusMovieInfoVisibilityProperty = DependencyProperty.Register(
            "RedboxPlusMovieInfoVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
            new FrameworkPropertyMetadata(Visibility.Collapsed)
            {
                AffectsRender = true
            });

        public static readonly DependencyProperty RedboxPlusOfferVisibilityProperty =
            DependencyProperty.Register("RedboxPlusOfferVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty OfferVisibilityProperty =
            DependencyProperty.Register("OfferVisibility", typeof(Visibility), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty OfferSeparatorLineVisibilityProperty =
            DependencyProperty.Register("OfferSeparatorLineVisibility", typeof(Visibility),
                typeof(ShoppingCartViewModel), new FrameworkPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty RedboxPlusOfferTextProperty =
            DependencyProperty.Register("RedboxPlusOfferText", typeof(string), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty RedboxPlusOfferHeaderTextProperty =
            DependencyProperty.Register("RedboxPlusOfferHeaderText", typeof(string), typeof(ShoppingCartViewModel),
                new FrameworkPropertyMetadata(null));

        public Func<ISpeechControl> OnGetSpeechControl;

        public ShoppingCartViewModel()
        {
            PricingModel = new PricingModel
            {
                SubtotalLineVisibility = Visibility.Visible,
                AddedDiscsLineVisibility = Visibility.Collapsed,
                ReservedDiscsLineVisibility = Visibility.Collapsed,
                PricingLineMargin = new Thickness(0.0, 7.0, 0.0, 7.0)
            };
        }

        public DynamicRoutedCommand RedboxPlusInfoCommand { get; set; }

        public string LabelTop { get; set; }

        public string PriceHeaderText { get; set; }

        public string BackButtonText { get; set; }

        public string SignInIncentiveText { get; set; }

        public string SignInButtonText1 { get; set; }

        public string SignInButtonText2 { get; set; }

        public string PassiveSaleNote { get; set; }

        public string MDVFootnote { get; set; }

        public string ServiceFeeText { get; set; }

        public string TermsAndPrivacyButtonText { get; set; }

        public string PromoCodeButtonText { get; set; }

        public string AddMovieButtonText { get; set; }

        public string UpdateBagButtonText { get; set; }

        public string RedboxPlusMovieInfoButtonText { get; set; }

        public string LoyaltySystemErrorText
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(LoyaltySystemErrorTextProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(LoyaltySystemErrorTextProperty, value); }); }
        }

        public Visibility LoyaltySystemErrorTextVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(LoyaltySystemErrorTextVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(LoyaltySystemErrorTextVisibilityProperty, value); }); }
        }

        public string PointsEstimateIntroText { get; set; }

        public string PointsEstimateText
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(PointsEstimateTextProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PointsEstimateTextProperty, value); }); }
        }

        public string PointsEstimateEndText { get; set; }

        public Visibility PointsEstimateVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(PointsEstimateVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PointsEstimateVisibilityProperty, value); }); }
        }

        public string PointsBalanceText
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(PointsBalanceTextProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PointsBalanceTextProperty, value); }); }
        }

        public string PointsBalanceInsufficientMessageText
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(PointsBalanceInsufficientMessageTextProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PointsBalanceInsufficientMessageTextProperty, value); }); }
        }

        public string PointsBalanceSufficientMessageText
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(PointsBalanceSufficientMessageTextProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PointsBalanceSufficientMessageTextProperty, value); }); }
        }

        public TextAlignment PointsBalanceInsufficientMessageTextAlignment
        {
            get
            {
                return Dispatcher.Invoke(() =>
                    (TextAlignment)GetValue(PointsBalanceInsufficientMessageTextAlignmentProperty));
            }
            set
            {
                Dispatcher.Invoke(delegate { SetValue(PointsBalanceInsufficientMessageTextAlignmentProperty, value); });
            }
        }

        public Visibility PointsBalanceStackPanelVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(PointsBalanceStackPanelVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PointsBalanceStackPanelVisibilityProperty, value); }); }
        }

        public Visibility PointsBalanceTextVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(PointsBalanceTextVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PointsBalanceTextVisibilityProperty, value); }); }
        }

        public Visibility PointsBalanceInfoButtonVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(PointsBalanceInfoButtonVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PointsBalanceInfoButtonVisibilityProperty, value); }); }
        }

        public Visibility PointsBalanceInsufficientMessageTextVisibility
        {
            get
            {
                return Dispatcher.Invoke(() =>
                    (Visibility)GetValue(PointsBalanceInsufficientMessageTextVisibilityProperty));
            }
            set
            {
                Dispatcher.Invoke(delegate
                {
                    SetValue(PointsBalanceInsufficientMessageTextVisibilityProperty, value);
                });
            }
        }

        public Visibility PointsBalanceSufficientMessageStackPanelVisibility
        {
            get
            {
                return Dispatcher.Invoke(() =>
                    (Visibility)GetValue(PointsBalanceSufficientMessageStackPanelVisibilityProperty));
            }
            set
            {
                Dispatcher.Invoke(delegate
                {
                    SetValue(PointsBalanceSufficientMessageStackPanelVisibilityProperty, value);
                });
            }
        }

        public string PayButtonText1
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(PayButtonText1Property)); }
            set { Dispatcher.Invoke(delegate { SetValue(PayButtonText1Property, value); }); }
        }

        public string PayButtonText2
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(PayButtonText2Property)); }
            set { Dispatcher.Invoke(delegate { SetValue(PayButtonText2Property, value); }); }
        }

        public Visibility PayButtonText1Visibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(PayButtonText1VisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PayButtonText1VisibilityProperty, value); }); }
        }

        public bool PayButtonEnabled
        {
            get { return Dispatcher.Invoke(() => (bool)GetValue(PayButtonEnabledProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PayButtonEnabledProperty, value); }); }
        }

        public string AcceptAndPayText
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(AcceptAndPayTextProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(AcceptAndPayTextProperty, value); }); }
        }

        public PricingModel PricingModel
        {
            get { return Dispatcher.Invoke(() => (PricingModel)GetValue(PricingModelProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PricingModelProperty, value); }); }
        }

        public Visibility ADASpacerVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(ADASpacerVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(ADASpacerVisibilityProperty, value); }); }
        }

        public Visibility AddMovieButtonVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(AddMovieButtonVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(AddMovieButtonVisibilityProperty, value); }); }
        }

        public bool PromoCodeButtonEnabled
        {
            get { return Dispatcher.Invoke(() => (bool)GetValue(PromoCodeButtonEnabledProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PromoCodeButtonEnabledProperty, value); }); }
        }

        public List<DisplayCheckoutProductModel> DisplayProductModels
        {
            get
            {
                return Dispatcher.Invoke(() =>
                    (List<DisplayCheckoutProductModel>)GetValue(DisplayProductModelsProperty));
            }
            set { Dispatcher.Invoke(delegate { SetValue(DisplayProductModelsProperty, value); }); }
        }

        public Visibility UpdateBagButtonVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(UpdateBagButtonVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(UpdateBagButtonVisibilityProperty, value); }); }
        }

        public Visibility SignInIncentiveTextVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(SignInIncentiveTextVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(SignInIncentiveTextVisibilityProperty, value); }); }
        }

        public Visibility SignInButtonVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(SignInButtonVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(SignInButtonVisibilityProperty, value); }); }
        }

        public Visibility UsePointsButtonVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(UsePointsButtonVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(UsePointsButtonVisibilityProperty, value); }); }
        }

        public bool UsePointsButtonEnabled
        {
            get { return Dispatcher.Invoke(() => (bool)GetValue(UsePointsButtonEnabledProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(UsePointsButtonEnabledProperty, value); }); }
        }

        public string UsePointsButtonText
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(UsePointsButtonTextProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(UsePointsButtonTextProperty, value); }); }
        }

        public Thickness PromoCodeButtonMargin
        {
            get { return Dispatcher.Invoke(() => (Thickness)GetValue(PromoCodeButtonMarginProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(PromoCodeButtonMarginProperty, value); }); }
        }

        public Visibility ServiceFeeTextVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(ServiceFeeTextVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(ServiceFeeTextVisibilityProperty, value); }); }
        }

        public Visibility MDVFootnoteVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(MDVFootnoteVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(MDVFootnoteVisibilityProperty, value); }); }
        }

        public Visibility OfferVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(OfferVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(OfferVisibilityProperty, value); }); }
        }

        public Visibility OfferSeparatorLineVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(OfferSeparatorLineVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(OfferSeparatorLineVisibilityProperty, value); }); }
        }

        public Visibility RedboxPlusOfferVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(RedboxPlusOfferVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(RedboxPlusOfferVisibilityProperty, value); }); }
        }

        public string RedboxPlusOfferText
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(RedboxPlusOfferTextProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(RedboxPlusOfferTextProperty, value); }); }
        }

        public string RedboxPlusOfferHeaderText
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(RedboxPlusOfferHeaderTextProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(RedboxPlusOfferHeaderTextProperty, value); }); }
        }

        public Visibility RedboxPlusMovieInfoVisibility
        {
            get { return Dispatcher.Invoke(() => (Visibility)GetValue(RedboxPlusMovieInfoVisibilityProperty)); }
            set { Dispatcher.Invoke(delegate { SetValue(RedboxPlusMovieInfoVisibilityProperty, value); }); }
        }

        public event Action OnBackButtonClicked;

        public event Action OnTermsAndPrivacyButtonClicked;

        public event Action OnPromoCodeButtonClicked;

        public event Action OnPayButtonClicked;

        public event Action OnAddMovieButtonClicked;

        public event Action OnUpdateBagButtonClicked;

        public event Action OnSignInButtonClicked;

        public event Action OnUsePointsButtonClicked;

        public event Action OnPointsBalanceInfoButtonClicked;

        public event Action OnLoaded;

        public event Action<DisplayProductModel, object> OnCancelBrowseItemModel;

        public event Action<DisplayProductModel> OnViewOfferDetailsButtonClicked;

        public event Action OnViewRedboxPlusOfferButtonClicked;

        public event Action OnScanRedboxPlusQRCodeButtonClicked;

        public void ProcessOnCancelBrowseItemModel(DisplayProductModel displayProductModel, object parameter)
        {
            if (OnCancelBrowseItemModel != null) OnCancelBrowseItemModel(displayProductModel, parameter);
        }

        public ISpeechControl ProcessGetSpeechControl()
        {
            ISpeechControl speechControl = null;
            if (OnGetSpeechControl != null) speechControl = OnGetSpeechControl();
            return speechControl;
        }

        public void ProcessOnSignInButtonClicked()
        {
            if (OnSignInButtonClicked != null) OnSignInButtonClicked();
        }

        public void ProcessOnBackButtonClicked()
        {
            if (OnBackButtonClicked != null) OnBackButtonClicked();
        }

        public void ProcessOnTermsAndPrivacyButtonClicked()
        {
            if (OnTermsAndPrivacyButtonClicked != null) OnTermsAndPrivacyButtonClicked();
        }

        public void ProcessOnPromoCodeButtonClicked()
        {
            if (OnPromoCodeButtonClicked != null) OnPromoCodeButtonClicked();
        }

        public void ProcessOnPayButtonClicked()
        {
            if (OnPayButtonClicked != null) OnPayButtonClicked();
        }

        public void ProcessOnAddMovieButtonClicked()
        {
            if (OnAddMovieButtonClicked != null) OnAddMovieButtonClicked();
        }

        public void ProcessOnUpdateBagButtonClicked()
        {
            var onUpdateBagButtonClicked = OnUpdateBagButtonClicked;
            if (onUpdateBagButtonClicked == null) return;
            onUpdateBagButtonClicked();
        }

        public void ProcessOnUsePointsButtonClicked()
        {
            var onUsePointsButtonClicked = OnUsePointsButtonClicked;
            if (onUsePointsButtonClicked == null) return;
            onUsePointsButtonClicked();
        }

        public void ProcessOnPointsBalanceInfoButtonClicked()
        {
            var onPointsBalanceInfoButtonClicked = OnPointsBalanceInfoButtonClicked;
            if (onPointsBalanceInfoButtonClicked == null) return;
            onPointsBalanceInfoButtonClicked();
        }

        public void ProcessOnViewOfferDetailsButtonClicked(DisplayProductModel displayProductModel)
        {
            var onViewOfferDetailsButtonClicked = OnViewOfferDetailsButtonClicked;
            if (onViewOfferDetailsButtonClicked == null) return;
            onViewOfferDetailsButtonClicked(displayProductModel);
        }

        public void ProcessOnLoaded()
        {
            var onLoaded = OnLoaded;
            if (onLoaded == null) return;
            onLoaded();
        }

        public void ProcessOnViewRedboxPlusOfferButtonClicked()
        {
            var onViewRedboxPlusOfferButtonClicked = OnViewRedboxPlusOfferButtonClicked;
            if (onViewRedboxPlusOfferButtonClicked == null) return;
            onViewRedboxPlusOfferButtonClicked();
        }

        public void ProcessOnScanRedboxPlusQRCodeButtonClicked()
        {
            var onScanRedboxPlusQRCodeButtonClicked = OnScanRedboxPlusQRCodeButtonClicked;
            if (onScanRedboxPlusQRCodeButtonClicked == null) return;
            onScanRedboxPlusQRCodeButtonClicked();
        }
    }
}