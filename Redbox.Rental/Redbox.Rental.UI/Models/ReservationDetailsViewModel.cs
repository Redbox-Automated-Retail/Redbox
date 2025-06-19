using System;
using System.Windows;
using Redbox.KioskEngine.ComponentModel.TextToSpeech;

namespace Redbox.Rental.UI.Models
{
    public class ReservationDetailsViewModel
    {
        public Func<ISpeechControl> OnGetSpeechControl;

        public string HeaderText { get; set; }

        public string BodyText { get; set; }

        public Visibility MDVVisibility { get; set; }

        public Visibility OkayButtonVisibility { get; set; }

        public string DiscVendText { get; set; }

        public string NoteHeaderText { get; set; }

        public string NoteBodyText { get; set; }

        public string ReturnWarningText { get; set; }

        public string OkayButtonText { get; set; }

        public bool CallbackExecuted { get; set; }
        public event Action OkayButtonAction;

        public void ProcessOkayButtonClick()
        {
            var okayButtonAction = OkayButtonAction;
            if (okayButtonAction == null) return;
            okayButtonAction();
        }

        public ISpeechControl ProcessGetSpeechControl()
        {
            ISpeechControl speechControl = null;
            if (OnGetSpeechControl != null) speechControl = OnGetSpeechControl();
            return speechControl;
        }
    }
}