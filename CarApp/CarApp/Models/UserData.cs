using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarApp.Models
{
    public class UserData : INotifyPropertyChanged
    {
        private string _firstname = "";
        private string _uid = "";
        private string _lastname = "";
        private DateTime _birthday = DateTime.Now;
        private string _gender = "";
        private string _address = "";
        private string _phone = "";
        private string _carCountry = "";
        private string _carBody = "";
        private string _carDrive = "";
        private string _transmission = "";
        private List<string> _favCars = new List<string>();

        public List<string> Genders { get; set; } = new List<string> { "Мужчина", "Женщина" };

        public List<string> CarCoutnries { get; set; } = new List<string>
        {
            "US", "DE", "GB", "FR", "IT", "ES", "CZ", "SE", "RU", "CN", "JP", "KR"
        };

        public List<string> CarBodyTypes { get; set; } = new List<string>
        {
            "", "Седан", "Хэтчбек", "Универсал", "Купе", "Кабриолет", "Внедорожник", "Кроссовер", "Минивэн", "Пикап",
            "Лифтбек", "Лимузин", "Фургон"
        };

        public List<string> CarDriverTypes { get; set; } = new List<string> { "", "FWD", "RWD", "AWD", "4D", "EV" };

        public List<string> TransmissionTypes { get; set; } = new List<string>
        {
            "", "Механическая", "Автоматическая", "Роботизированная", "Вариатор", "Полуавтоматическая",
            "Двухсцепной робот"
        };

        public string Uid
        {
            get => _uid;
            set => SetField(ref _uid, value);
        }

        public string Firstname
        {
            get => _firstname;
            set => SetField(ref _firstname, value);
        }

        public string Lastname
        {
            get => _lastname;
            set => SetField(ref _lastname, value);
        }

        public DateTime Birthday
        {
            get => _birthday;
            set => SetField(ref _birthday, value);
        }

        public string Gender
        {
            get => _gender;
            set => SetField(ref _gender, value);
        }

        public string Address
        {
            get => _address;
            set => SetField(ref _address, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetField(ref _phone, value);
        }

        public string CarCountry
        {
            get => _carCountry;
            set => SetField(ref _carCountry, value);
        }

        public string CarBody
        {
            get => _carBody;
            set => SetField(ref _carBody, value);
        }

        public string CarDrive
        {
            get => _carDrive;
            set => SetField(ref _carDrive, value);
        }

        public string Transmission
        {
            get => _transmission;
            set => SetField(ref _transmission, value);
        }

        public List<string> FavCars
        {
            get => _favCars;
            set => SetField(ref _favCars, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}