using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly ICarsService _carsService;
        public event PropertyChangedEventHandler PropertyChanged;

        private string _text = "signOut";
        public string CheckText
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public ObservableCollection<Car> Cars { get; set; }
        public ICommand SignOutCommand { protected set; get; }
        public ICommand ToggleLikeCommand { protected set; get; }
        public ICommand ItemTappedCommand { protected set; get; }

        private INavigation Navigation { get; set; }

        public HomeViewModel(INavigation navigation)
        {
            Navigation = navigation;

            _authService = DependencyService.Get<AuthService>();
            _carsService = DependencyService.Get<ICarsService>();

            Cars = _carsService.Cars;

            SignOutCommand = new Command(SignOut);
            ItemTappedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(OpenCarDetailedPage);
            ToggleLikeCommand = new Command<Car>(car =>
            {
                car.IsFav = !car.IsFav;
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
            _authService.AuthState -= AuthStateHandler;
            Navigation.PushAsync(page);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}