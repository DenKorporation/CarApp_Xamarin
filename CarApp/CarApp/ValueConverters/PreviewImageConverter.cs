using System;
using System.Globalization;
using System.Threading.Tasks;
using CarApp.Abstractions;
using CarApp.Models;
using Xamarin.Forms;

namespace CarApp.ValueConverters
{
    public class PreviewImageConverter : AsyncConverter<string>
    {
        private IStorageService _storageService = DependencyService.Get<IStorageService>();

        public override async Task<string> AsyncConvert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (value is Car car)
            {
                return await _storageService.GetPreviewCarImageUrl(car);
            }

            return null;
        }
    }
}