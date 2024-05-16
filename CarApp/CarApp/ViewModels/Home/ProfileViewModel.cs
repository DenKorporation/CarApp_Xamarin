using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarApp.Abstractions;
using CarApp.Models;
using CarApp.Views.Authentication;
using CarApp.Views.Home;
using Xamarin.Forms;

namespace CarApp.ViewModels.Home
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly IUserDataService _userDataService;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _displayPopup;
        private bool _countryPopup;

        public bool DisplayPopup
        {
            get => _displayPopup;
            set => SetField(ref _displayPopup, value);
        }

        public bool CountryPopup
        {
            get => _countryPopup;
            set => SetField(ref _countryPopup, value);
        }


        public UserData SelectedUserData { get; set; } = new UserData();

        public UserData UserData { get; set; }
        public ICommand SignOutCommand { protected set; get; }
        public ICommand MenuTappedCommand { protected set; get; }

        public ICommand ToHomePageCommand { protected set; get; }

        public ICommand ToFavPageCommand { protected set; get; }
        public ICommand UpdateCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }


        private INavigation Navigation { get; set; }

        public ProfileViewModel(INavigation navigation)
        {
            Console.WriteLine("ProfileViewModel Ctor");
            Navigation = navigation;

            _authService = DependencyService.Get<AuthService>();
            _userDataService = DependencyService.Get<IUserDataService>();
            _userDataService.Uid = _authService.CurrentUser.Uid;

            UserData = _userDataService.UserData;

            var userDataType = UserData.GetType();
            var properties = userDataType.GetProperties();
            foreach (var property in properties)
            {
                if (property.GetValue(UserData) != property.GetValue(SelectedUserData))
                {
                    property.SetValue(SelectedUserData, property.GetValue(UserData));
                }
            }

            SignOutCommand = new Command(SignOut);
            ToHomePageCommand = new Command(() =>
            {
                DisplayPopup = false;
                PushPage(new HomePage());
            });
            ToFavPageCommand = new Command(() =>
            {
                DisplayPopup = false;
                PushPage(new FavPage());
            });
            MenuTappedCommand = new Command(() => { DisplayPopup = true; });
            UpdateCommand = new Command(async () =>
            {
                await _userDataService.UpdateUserDataAsync(SelectedUserData.Firstname, SelectedUserData.Lastname,
                    SelectedUserData.Birthday, SelectedUserData.Gender, SelectedUserData.Address,
                    SelectedUserData.Phone,
                    SelectedUserData.CarCountry, SelectedUserData.CarBody, SelectedUserData.CarDrive,
                    SelectedUserData.Transmission);
                PushPage(new HomePage());
            });
            DeleteCommand = new Command(async () =>
            {
                _authService.DeleteAccount();
                PushPage(new SignInPage());
            });

            if (_authService.CurrentUser == null)
            {
                Navigation.PushAsync(new SignInPage());
            }
            else
            {
                _authService.AuthState += AuthStateHandler;
            }
        }

        private void AuthStateHandler(object sender, AuthStateEventArgs e)
        {
            if (e.User?.Uid == null)
            {
                PushPage(new SignInPage());
            }
        }

        private void SignOut()
        {
            _authService.SignOut();
            DisplayPopup = false;
        }

        private void PushPage(Page page)
        {
            _authService.AuthState -= AuthStateHandler;
            Navigation.PushAsync(page);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}