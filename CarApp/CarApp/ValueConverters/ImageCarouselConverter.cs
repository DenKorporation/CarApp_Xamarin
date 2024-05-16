using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CarApp.Abstractions;
using CarApp.Models;
using Xamarin.Forms;

namespace CarApp.ValueConverters
{
    public class ImageCarouselConverter : AsyncConverter<List<string>>
    {
        private IStorageService _storageService = DependencyService.Get<IStorageService>();

        public override async Task<List<string>> AsyncConvert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (value is Car car)
            {
                return await _storageService.GetAllCarImageUrls(car);
            }

            return null;
        }
    }
}