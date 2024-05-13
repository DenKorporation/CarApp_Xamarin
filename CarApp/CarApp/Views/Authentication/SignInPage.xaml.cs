using System;
using CarApp.Abstractions;
using CarApp.Models;
using CarApp.ViewModels.Authentication;
using CarApp.Views.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarApp.Views.Authentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
            BindingContext = new AuthenticationViewModel(Navigation);
        }
    }
}