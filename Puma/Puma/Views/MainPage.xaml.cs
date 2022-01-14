using Client.Models;
using Client.Services;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    public partial class MainPage : ContentPage
    {
        UserApiService _userApiService = new UserApiService();
        public MainPage()
        {
            InitializeComponent();
            _userApiService.ErrorMessage += ReportErrorMessage;
        }
        void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            PopUp.Pinmethod(map, e);
        }

        private void btn_closePopup(object sender, EventArgs e)
        {
            loginPopup.IsVisible = false;
            signupPopup.IsVisible = false;
        }

        private async void btn_Signup_Popup_Clicked(object sender, EventArgs e)
        {
            PopUp.PublicVisibilityforTwo(signupPopup, loginPopup);
        }

        private async void btn_Login_Popup_Clicked(object sender, EventArgs e)
        {
            PopUp.PublicVisibilityforTwo(loginPopup, signupPopup);
        }

        //metoder

        private void btn_login_Clicked(object sender, EventArgs e)
        {

        }

        private async void btn_signup_Clicked(object sender, EventArgs e)
        {
            PopUp.RegisterUser(txt_signupEmail, txt_signupPassword, txt_signupDisplayName, txt_signupFirstName, txt_signupSurname, _userApiService);
        }

        private void ReportErrorMessage(object sender, string message) => DisplayAlert("Error", $"{message}", "OK");
    }
}
