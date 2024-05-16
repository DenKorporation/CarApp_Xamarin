using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarApp.Abstractions;
using CarApp.Models;
using CarApp.Views.Authentication;
using CarApp.Views.Home;
using Rg.Plugins.Popup.Extensions;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace CarApp.ViewModels.Home
{
    public class FavViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly ICarsService _carsService;
        private readonly IUserDataService _userDataService;
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _displayPopup;

        public bool DisplayPopup
        {
            get => _displayPopup;
            set
            {
                _displayPopup = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Car> Cars { get; set; }
        public ObservableCollection<Car> OnlyFavCars { get; set; } = new ObservableCollection<Car>();
        public UserData UserData { get; set; }
        public ICommand SignOutCommand { protected set; get; }
        public ICommand ToggleLikeCommand { protected set; get; }
        public ICommand ItemTappedCommand { protected set; get; }
        public ICommand MenuTappedCommand { protected set; get; }
        public ICommand ToHomePageCommand { protected set; get; }
        public ICommand ToProfilePageCommand { protected set; get; }


        private INavigation Navigation { get; set; }

        public FavViewModel(INavigation navigation)
        {
            Navigation = navigation;

            _authService = DependencyService.Get<AuthService>();
            _carsService = DependencyService.Get<ICarsService>();
            _userDataService = DependencyService.Get<IUserDataService>();
            _userDataService.Uid = _authService.CurrentUser.Uid;

            Cars = _carsService.Cars;
            UserData = _userDataService.UserData;

            foreach (var car in Cars)
            {
                car.IsFav = UserData.FavCars.Contains(car.Uid);
                if (car.IsFav)
                {
                    OnlyFavCars.Add(car);
                }
            }
            
            UserData.PropertyChanged += OnUserDataOnPropertyChanged;

            SignOutCommand = new Command(SignOut);
            ItemTappedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(OpenCarDetailedPage);
            ToProfilePageCommand = new Command(() =>
            {
                DisplayPopup = false;
                PushPage(new ProfilePage());
            });
            ToHomePageCommand = new Command(() =>
            {
                DisplayPopup = false;
                PushPage(new HomePage());
            });
            ToggleLikeCommand = new Command<Car>(car =>
            {
                List<string> copy = UserData.FavCars.ToArray().ToList();
                if (copy.Contains(car.Uid))
                {
                    OnlyFavCars.Remove(car);
                    copy.Remove(car.Uid);
                }
                else
                {
                    copy.Add(car.Uid);
                }

                car.IsFav = !car.IsFav;
                _userDataService.UpdateUserFavCarsAsync(copy);
            });
            MenuTappedCommand = new Command(() => { DisplayPopup = true; });

            if (_authService.CurrentUser == null)
            {
                UserData.PropertyChanged -= OnUserDataOnPropertyChanged;
                Navigation.PushAsync(new SignInPage());
            }
            else
            {
                _authService.AuthState += AuthStateHandler;
            }
        }

        private void OnUserDataOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(UserData.FavCars))
            {
                foreach (var car in Cars)
                {
                    if (UserData.FavCars.Contains(car.Uid))
                    {
                        if (!OnlyFavCars.Contains(car))
                        {
                            OnlyFavCars.Add(car);
                        }
                        car.IsFav = true;
                    }
                    else
                    {
                        if (OnlyFavCars.Contains(car))
                        {
                            OnlyFavCars.Remove(car);
                        }
                        car.IsFav = false;
                    }
                }
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

        private void OpenCarDetailedPage(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            if (e.ItemType is ItemType.Record && e.ItemData is Car car)
            {
                Navigation.PushPopupAsync(new CarDetailedPage(car));
            }
        }

        private void PushPage(Page page)
        {
            UserData.PropertyChanged -= OnUserDataOnPropertyChanged;
            _authService.AuthState -= AuthStateHandler;
            Navigation.PushAsync(page);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}