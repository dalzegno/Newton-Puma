using Client.Services;
using Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Client.Views
{
    public partial class MainPage : ContentPage
    {
        IUserApiService UserApiService => DependencyService.Get<IUserApiService>();
        IDialogService DialogService => DependencyService.Get<IDialogService>();

        public MainPage()
        {
            InitializeComponent();

            // Implementing dependecy injection
            BindingContext = new MainViewModel(UserApiService);
            slCreateUserViewModel.BindingContext = new NewUserViewModel(UserApiService, DialogService);
            slLogIn.BindingContext = new LoginViewModel(UserApiService, DialogService);
        }

        void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            PopUp.Pinmethod(map, e);
        }


    }
}
