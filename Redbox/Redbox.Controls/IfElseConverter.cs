using System;
using System.Globalization;
using System.Windows.Data;

namespace Redbox.Controls
{
    public class IfElseConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)values[0] ? values[1] : values[2];
        }

        public object[] ConvertBack(
            object values,
            Type[] targetType,
            object parameter,
            CultureInfo culture)
        {
            return (object[])null;
        }
    }
}