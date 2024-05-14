using System;
using CarApp.ViewModels.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel(Navigation);
        }
    }
}