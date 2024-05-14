using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarApp.Models
{
    public class UserData : INotifyPropertyChanged
    {
        private string _firstname;
        private string _uid;
        private string _lastname;
        private DateTime _birthday;
        private string _gender;
        private string _address;
        private string _phone;
        private string _carCountry;
        private string _carBody;
        private string _carDrive;
        private string _transmission;
        private List<string> _favCars;

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

        public List<String> FavCars
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