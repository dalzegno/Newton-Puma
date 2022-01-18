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

        //private void btn_closePopup(object sender, EventArgs e)
        //{
        //    loginPopup.IsVisible = false;
        //    signupPopup.IsVisible = false;
        //}

        //private async void btn_Signup_Popup_Clicked(object sender, EventArgs 
        //{
        //    PopUp.PublicVisibilityforTwo(signupPopup, loginPopup);
        //}

        //private async void btn_Login_Popup_Clicked(object sender, EventArgs e)
        //{
        //    PopUp.PublicVisibilityforTwo(loginPopup, signupPopup);
        //}

        ////metoder

        //private void btn_login_Clicked(object sender, EventArgs e)
        //{

        //}

        ////private async void btn_signup_Clicked(object sender, EventArgs e)
        ////{
        ////    PopUp.RegisterUser(txt_signupEmail, txt_signupPassword, txt_signupDisplayName, txt_signupFirstName, txt_signupSurname, _userApiService);
        ////}


    }
}
