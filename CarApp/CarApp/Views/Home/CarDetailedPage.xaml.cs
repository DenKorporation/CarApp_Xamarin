using CarApp.Models;
using Xamarin.Forms.Xaml;

namespace CarApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]  
    public partial class CarDetailedPage : Rg.Plugins.Popup.Pages.PopupPage  
    {  
        public CarDetailedPage(Car car)  
        {  
            InitializeComponent();
            BindingContext = car;
        }  
    }  
}