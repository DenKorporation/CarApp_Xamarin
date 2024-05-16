using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarApp.ValueConverters
{
    public class CountryCodeToName: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RegionInfo regionInfo = new RegionInfo(value.ToString());
        
            
            return regionInfo.DisplayName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}