using System;
using System.Windows;

namespace Redbox.Rental.UI.Models
{
    public class KeypadModel : DependencyObject
    {
        public Action OnBackButtonClicked;

        public Action OnClearAllButtonClicked;

        public Action<string> OnNumberButtonClicked;
        public string ClearAllButtonText { get; set; }

        public string BackButtonText { get; set; }

        public void ProcessOnNumberButtonClicked(string key)
        {
            if (OnNumberButtonClicked != null) OnNumberButtonClicked(key);
        }

        public void ProcessOnClearAllButtonClicked()
        {
            if (OnClearAllButtonClicked != null) OnClearAllButtonClicked();
        }

        public void ProcessOnBackButtonClicked()
        {
            if (OnBackButtonClicked != null) OnBackButtonClicked();
        }
    }
}