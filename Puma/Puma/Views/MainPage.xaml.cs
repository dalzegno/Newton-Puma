using Client.Models;
using Client.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Puma
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
            map.Pins.Clear();
            Pin pin = new Pin
            {

                Label = "",
                Address = "",
                Type = PinType.Generic,
                Position = new Position(e.Position.Latitude, e.Position.Longitude)
            };
            map.Pins.Add(pin);
            System.Diagnostics.Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");
        }

        private void btn_closePopup(object sender, EventArgs e)
        {
            loginPopup.IsVisible = false;
            signupPopup.IsVisible = false;
        }

        private async void btn_Signup_Popup_Clicked(object sender, EventArgs e)
        {
            signupPopupVisible();
            //var users = await _userApiService.GetUsersAsync();
            //foreach (var user in users)
            //{
            //    System.Diagnostics.Debug.WriteLine($"User firstname: {user.FirstName}, User lastname: {user.LastName}");
            //}
        }

        private async void btn_Login_Popup_Clicked(object sender, EventArgs e)
        {
            loginPopupVisible();
            
        }

        //metoder
        public void loginPopupVisible()
        {
            signupPopup.IsVisible = false;
            loginPopup.IsVisible = true;
        }
        public void signupPopupVisible()
        {
            loginPopup.IsVisible = false;
            signupPopup.IsVisible = true;
        }

        private void btn_login_Clicked(object sender, EventArgs e)
        {

        }

        private async void btn_signup_Clicked(object sender, EventArgs e)
        {
            var user = new UserDto()
            {
                Email = txt_signupEmail?.Text,
                Password = txt_loginPassword?.Text,
                DisplayName = txt_signupDisplayName?.Text,
                FirstName = txt_signupFirstName?.Text ?? "",
                LastName = txt_signupSurname?.Text ?? ""
            };


            var createdUser = await _userApiService.CreateUserAsync(user);
            if (createdUser != null)
                System.Diagnostics.Debug.WriteLine($"User created!: {createdUser.FirstName}, User lastname: {createdUser.LastName}");
        }

        private void ReportErrorMessage(object sender, string message) => DisplayAlert("Error", $"{message}", "OK");
    }
}
