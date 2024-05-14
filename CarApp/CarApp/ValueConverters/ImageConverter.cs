using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarApp.ValueConverters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "flag_" + value.ToString().ToLower() + ".png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}