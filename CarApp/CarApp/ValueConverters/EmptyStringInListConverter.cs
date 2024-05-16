using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace CarApp.ValueConverters
{
    public class EmptyStringInListConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as List<string>;

            var result = new List<string>();
            
            foreach (var val in list)
            {
                result.Add(string.IsNullOrEmpty(val) ? "Нет предпочтений" : val);
            }
            
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}