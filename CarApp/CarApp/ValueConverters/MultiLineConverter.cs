using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarApp.ValueConverters
{
    public class MultiLineConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Replace('/', '\n');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}