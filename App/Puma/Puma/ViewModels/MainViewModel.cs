using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Puma.Views;
using Puma.Services;
using Puma.Models;

namespace Puma.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private IUserApiService _userApiService;
        public MainViewModel(IUserApiService userApiService)
        {
            _userApiService = userApiService;
            ClosePopupCommand = new Command(ClosePopup);
            OpenSignupCommand = new Command(OpenSignup);
            OpenLoginCommand = new Command(OpenLogin);
            OpenSettingsCommand = new Command(OpenSettings);
            UserLoggedInCommand = new Command(UserLoggedIn);

        }
        #region Popup states for Login and Signup
        public ICommand ClosePopupCommand { get; }
        public ICommand OpenSignupCommand { get; }
        public ICommand OpenLoginCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand UserLoggedInCommand { get; }

        public bool openLoginBool { get; set; } = false;
        public bool openSignupBool { get; set; } = false;
        public bool openSettingsBool { get; set; } = false;
        public bool isUserLoggedOut { get; set; } = true;
        
        public void UserLoggedIn()
        {
            if (StaticUser.LoggedInUser == null)
            {
                isUserLoggedOut = true;
                OnPropertyChanged(nameof(userLoginState));
                return;
            }

            isUserLoggedOut = false;
            openLoginBool = false;
            OnPropertyChanged(nameof(userLoginState));
            OnPropertyChanged(nameof(loginPopupState));
        }
        public void ClosePopup()
        {
            openLoginBool = false;
            openSignupBool = false;
            openSettingsBool = false;
            OnPropertyChanged(nameof(signupPopupState));
            OnPropertyChanged(nameof(loginPopupState));
            OnPropertyChanged(nameof(settingsPopupState));
        }
        public void OpenLogin()
        {
            openLoginBool = true;
            openSignupBool = false;
            openSettingsBool = false;
            OnPropertyChanged(nameof(signupPopupState));
            OnPropertyChanged(nameof(loginPopupState));
            OnPropertyChanged(nameof(settingsPopupState));
        }
        void OpenSignup()
        {
            openSignupBool = true;
            openLoginBool = false;
            openSettingsBool = false;
            OnPropertyChanged(nameof(loginPopupState));
            OnPropertyChanged(nameof(signupPopupState));
            OnPropertyChanged(nameof(settingsPopupState));
        }
        void OpenSettings()
        {
            openSignupBool = false;
            openLoginBool = false;
            openSettingsBool = true;
            OnPropertyChanged(nameof(signupPopupState));
            OnPropertyChanged(nameof(loginPopupState));
            OnPropertyChanged(nameof(settingsPopupState));
        }

        public bool signupPopupState => openSignupBool;
        public bool loginPopupState => openLoginBool;
        public bool settingsPopupState => openSettingsBool;
        public bool userLoginState => isUserLoggedOut;

        #endregion
        
        //void OnMapClicked(object sender, MapClickedEventArgs e)
        //{
        //    map.Pins.Clear();
        //    Pin pin = new Pin
        //    {

        //        Label = "",
        //        Address = "",
        //        Type = PinType.Generic,
        //        Position = new Position(e.Position.Latitude, e.Position.Longitude)
        //    };
        //    map.Pins.Add(pin);
        //    System.Diagnostics.Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");

        //    Latitude = $"latitude: {e.Position.Latitude}";
        //    Longitude = $"longitude: {e.Position.Longitude}";
        //}

    }
}
