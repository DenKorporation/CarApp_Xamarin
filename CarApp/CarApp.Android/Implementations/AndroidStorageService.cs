using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using CarApp.Abstractions;
using CarApp.Droid.Implementations;
using CarApp.Models;
using Firebase.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidStorageService))]

namespace CarApp.Droid.Implementations
{
    public class AndroidStorageService : IStorageService
    {
        FirebaseStorage _firebaseStorage = FirebaseStorage.Instance;
        
        public async Task<string?> GetPreviewCarImageUrl(Car car)
        { 
            return (await _firebaseStorage.GetReference($"{car.Name}/preview.jpg").GetDownloadUrlAsync()).ToString();
        }

        public async Task<List<string>> GetAllCarImageUrls(Car car)
        {
           try
           {
               List<string> result = new List<string>();
               var storageRef = _firebaseStorage.GetReference($"{car.Name}/detailed_photos");
               ListResult  listResult = (ListResult)await storageRef.ListAll();

               foreach (var item in listResult.Items)
               {
                   var downloadUrl = await item.GetDownloadUrlAsync();
                   result.Add(downloadUrl.ToString());
               }

               return result;
           }
           catch (StorageException e)
           {
               Console.WriteLine($"Failed with error '{e.ErrorCode}': {e.Message}");
               return new List<string>();
           }
        }
    }
}