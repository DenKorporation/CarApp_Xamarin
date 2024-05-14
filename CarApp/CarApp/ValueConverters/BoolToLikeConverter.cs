using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarApp.ValueConverters
{
    public class BoolToLikeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "FAS" : "FAR";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}