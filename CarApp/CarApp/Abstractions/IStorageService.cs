using System.Collections.Generic;
using System.Threading.Tasks;
using CarApp.Models;

namespace CarApp.Abstractions
{
    public interface IStorageService
    {
        Task<string> GetPreviewCarImageUrl(Car car);
        Task<List<string>> GetAllCarImageUrls(Car car);
    }
}