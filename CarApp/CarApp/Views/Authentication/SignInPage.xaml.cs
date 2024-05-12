using System;
using CarApp.Abstractions;
using CarApp.Models;
using CarApp.Views.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarApp.Views.Authentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        private readonly AuthService _authService;

        public SignInPage()
        {
            InitializeComponent();

            _authService = DependencyService.Get<AuthService>();
            
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

        private void OnSignInClicked(object sender, EventArgs e)
        {
            _authService.SignInWithEmailAndPassword(EmailEntry.Text, PasswordEntry.Text);
        }

        private void OnRegisterClicked(object sender, EventArgs e)
        {
            
            PushPage(new RegistrationPage());
        }

        private void PushPage(Page page)
        {
            _authService.AuthState -= AuthStateHandler;
            Navigation.PushAsync(page);
        }
    }
}