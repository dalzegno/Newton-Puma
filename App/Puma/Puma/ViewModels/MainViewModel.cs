using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Client.Views;
using Client.Services;

namespace Client.ViewModels
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
        }
        #region Popup states for Login and Signup
        public ICommand ClosePopupCommand { get; }
        public ICommand OpenSignupCommand { get; }
        public ICommand OpenLoginCommand { get; }

        public bool openLoginBool { get; set; } = false;
        public bool openSignupBool { get; set; } = false;
        public void ClosePopup()
        {
            openLoginBool = false;
            openSignupBool = false;
            OnPropertyChanged(nameof(signupPopupState));
            OnPropertyChanged(nameof(loginPopupState));
        }
        public void OpenLogin()
        {
            openLoginBool = true;
            openSignupBool = false;
            OnPropertyChanged(nameof(signupPopupState));
            OnPropertyChanged(nameof(loginPopupState));
        }
        void OpenSignup()
        {
            openSignupBool = true;
            openLoginBool = false;
            OnPropertyChanged(nameof(loginPopupState));
            OnPropertyChanged(nameof(signupPopupState));
        }
        public bool signupPopupState => openSignupBool;
        public bool loginPopupState => openLoginBool;

        #endregion

        
    }
}
