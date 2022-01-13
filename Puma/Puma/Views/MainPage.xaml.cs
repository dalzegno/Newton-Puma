using Client.Models;
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
       
        public MainPage()
        {
            
            InitializeComponent();
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

        private void btn_Signup_Popup_Clicked(object sender, EventArgs e)
        {
            signupPopupVisible();
        }

        private void btn_Login_Popup_Clicked(object sender, EventArgs e)
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

        private void btn_signup_Clicked(object sender, EventArgs e)
        {
            var user = new UserDto()
            {
                Email = txt_signupEmail?.Text,
                Password = txt_loginPassword?.Text,
                DisplayName = txt_signupDisplayName?.Text,
                FirstName = txt_signupFirstName?.Text ?? "",
                LastName = txt_signupSurname?.Text ?? ""
            };


        }
    }
}
