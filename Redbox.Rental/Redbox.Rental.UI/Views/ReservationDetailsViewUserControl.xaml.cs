using System.Windows.Input;
using Redbox.KioskEngine.ComponentModel;
using Redbox.KioskEngine.ComponentModel.TextToSpeech;
using Redbox.Rental.UI.Controls;
using Redbox.Rental.UI.Models;

namespace Redbox.Rental.UI.Views
{
    [ThemedControl(ThemeName = "Redbox Classic", ControlName = "ReservationDetailsView")]
    public partial class ReservationDetailsViewUserControl : TextToSpeechUserControl, IWPFActor
    {
        public ReservationDetailsViewUserControl()
        {
            InitializeComponent();
        }

        private ReservationDetailsViewModel Model => DataContext as ReservationDetailsViewModel;

        public override ISpeechControl GetSpeechControl()
        {
            var model = Model;
            if (model == null) return null;
            return model.ProcessGetSpeechControl();
        }

        private void OkayButtonCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var model = Model;
            if (model != null) model.ProcessOkayButtonClick();
            HandleWPFHit();
        }
    }
}