using Redbox.KioskEngine.ComponentModel;
using Redbox.KioskEngine.ComponentModel.TextToSpeech;
using Redbox.Rental.UI.Controls;
using Redbox.Rental.UI.Models;

namespace Redbox.Rental.UI.Views
{
    [ThemedControl(ThemeName = "Redbox Classic", ControlName = "RedboxPlusTermsAcceptanceView")]
    public partial class RedboxPlusTermsAcceptanceViewUserControl : TextToSpeechUserControl, IWPFActor
    {
        public RedboxPlusTermsAcceptanceViewUserControl()
        {
            InitializeComponent();
        }

        private RedboxPlusTermsAcceptanceViewModel RedboxPlusTermsAcceptanceViewModel =>
            DataContext as RedboxPlusTermsAcceptanceViewModel;

        public override ISpeechControl GetSpeechControl()
        {
            var redboxPlusTermsAcceptanceViewModel = RedboxPlusTermsAcceptanceViewModel;
            if (redboxPlusTermsAcceptanceViewModel == null) return null;
            return redboxPlusTermsAcceptanceViewModel.ProcessGetSpeechControl();
        }
    }
}