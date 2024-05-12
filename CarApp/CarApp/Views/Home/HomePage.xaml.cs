using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarApp.Abstractions;
using CarApp.Models;
using CarApp.Views.Authentication;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly AuthService _authService;

        public HomePage()
        {
            InitializeComponent();

            _authService = DependencyService.Get<AuthService>();
            
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
        
        private void OnSignOutClicked(object sender, EventArgs e)
        {
            _authService.SignOut();
        }
        
        private void PushPage(Page page)
        {
            _authService.AuthState -= AuthStateHandler;
            Navigation.PushAsync(page);
        }
    }
}