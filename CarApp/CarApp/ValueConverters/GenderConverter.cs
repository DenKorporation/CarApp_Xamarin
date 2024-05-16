using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace CarApp.ValueConverters
{
    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedGender = value as string;

            return selectedGender == "Женщина" ? 1 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = value is int ? (int)value : 0;
            return index == 0 ? "Мужчина" : "Женщина";
        }
    }
}