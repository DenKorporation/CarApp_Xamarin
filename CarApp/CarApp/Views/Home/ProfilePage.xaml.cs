using CarApp.ViewModels.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SelectionChangedEventArgs = Syncfusion.XForms.Buttons.SelectionChangedEventArgs;

namespace CarApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel(Navigation);
        }

        private void SfSegmentedControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BindingContext is ProfileViewModel viewModel)
            {
                viewModel.SelectedUserData.Gender = viewModel.SelectedUserData.Genders[e.Index];
            }
        }
    }
}