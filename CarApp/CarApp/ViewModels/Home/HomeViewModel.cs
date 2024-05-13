using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarApp.Abstractions;
using CarApp.Models;
using CarApp.Views.Authentication;
using Xamarin.Forms;

namespace CarApp.ViewModels.Home
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly ICarsService _carsService;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Car> Cars { get; set; }
        public ICommand SignOutCommand { protected set; get; }

        private INavigation Navigation { get; set; }

        public HomeViewModel(INavigation navigation)
        {
            Navigation = navigation;

            _authService = DependencyService.Get<AuthService>();
            _carsService = DependencyService.Get<ICarsService>();

            Cars = _carsService.Cars;

            SignOutCommand = new Command(SignOut);


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
    }
}