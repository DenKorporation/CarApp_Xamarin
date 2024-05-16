using CarApp.ViewModels.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavPage : ContentPage
    {
        public FavPage()
        {
            InitializeComponent();
            BindingContext = new FavViewModel(Navigation);
        }
    }
}