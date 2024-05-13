using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarApp.Abstractions;
using CarApp.Models;
using CarApp.Views.Authentication;
using CarApp.Views.Home;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarApp.ViewModels.Authentication
{
    public class AuthenticationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private readonly AuthService _authService;

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ToRegisterPageCommand { protected set; get; }
        public ICommand ToSignInPageCommand { protected set; get; }
        public ICommand RegisterCommand { protected set; get; }
        public ICommand SignInCommand { protected set; get; }
        
        public INavigation Navigation { get; set;}
        
        public AuthenticationViewModel(INavigation navigation)
        {
            Navigation = navigation;
            _authService = DependencyService.Get<AuthService>();
            ToRegisterPageCommand = new Command(ToRegisterPage);
            ToSignInPageCommand = new Command(ToSignInPage);
            RegisterCommand = new Command(Register);
            SignInCommand = new Command(SignIn);
            
            if (_authService.CurrentUser != null)
            {
                Navigation.PushAsync(new HomePage());
            }
            else
            {
                _authService.AuthState += AuthStateHandler;
            }
        }
        
        private void AuthStateHandler(object sender, AuthStateEventArgs e)
        {
            if (e.User?.Uid != null)
            {
                PushPage(new HomePage());
            }
        }
        
        private void Register()
        {
            _authService.RegisterWithEmailAndPassword(Email, Password);
        }

        private void SignIn()
        {
            _authService.SignInWithEmailAndPassword(Email, Password);
        }

        private void ToSignInPage()
        {
            PushPage(new SignInPage());
        }
        
        private void ToRegisterPage()
        {
            PushPage(new RegistrationPage());
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