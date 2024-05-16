using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarApp.Models;

namespace CarApp.Abstractions
{
    public interface IUserDataService
    {
        string Uid { get; set; }
        UserData UserData { get; }
        Task CreateUserDataAsync();
        Task UpdateUserDataAsync(string firstname, string lastname, DateTime birthday, string gender, string address,
            string phone, string carCountry, string carBody, string carDrive, string transmission);
        Task UpdateUserFavCarsAsync(List<String> favCars);
        Task DeleteUserDataAsync();
    }
}