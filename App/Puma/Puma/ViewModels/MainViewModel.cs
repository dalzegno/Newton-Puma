
using System.Windows.Input;
using Xamarin.Forms;

using Puma.Services;
using Puma.Models;
using Puma.Views;

namespace Puma.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private IDialogService _dialogService;
        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            ClosePopupCommand = new Command(ClosePopup);
            OpenSignupCommand = new Command(OpenSignup);
            OpenLoginCommand = new Command(OpenLogin);
            OpenSettingsCommand = new Command(OpenSettings);
            UserLoggedInCommand = new Command(UserLoggedIn);
            LogOutCommand = new Command(LogOut);

        }
        #region Popup states for Login and Signup
        public ICommand ClosePopupCommand { get; }
        public ICommand OpenSignupCommand { get; }
        public ICommand OpenLoginCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand UserLoggedInCommand { get; }
        public ICommand LogOutCommand { get; }

        public bool openLoginBool { get; set; } = false;
        public bool openSignupBool { get; set; } = false;
        public bool openSettingsBool { get; set; } = false;
        public bool isUserLoggedOut { get; set; } = true;
        public bool isUserLoggedIn { get; set; } = false;
        public bool editSettingsVisible { get; set; } = false;

        public void UserLoggedIn()
        {
            if (App.LoggedInUser == null)
            {
                isUserLoggedOut = true;
                isUserLoggedIn = false;
                EditSettingsVisible = false;
                MainPage.Instance.PoiViewModel.PoiCommentPostVisible = false;
                OnPropertyChanged(nameof(MainPage.Instance.PoiViewModel.PoiCommentPostVisible));
                OnPropertyChanged(nameof(userLoginState));
                OnPropertyChanged(nameof(userLogoutState));
                OnPropertyChanged(nameof(EditSettingsVisible));
                return;
            }

            isUserLoggedOut = false;
            isUserLoggedIn = true;
            openLoginBool = false;
            EditSettingsVisible = true;
            MainPage.Instance.PoiViewModel.PoiCommentPostVisible = true;
            OnPropertyChanged(nameof(MainPage.Instance.PoiViewModel.PoiCommentPostVisible));
            OnPropertyChanged(nameof(userLoginState));
            OnPropertyChanged(nameof(userLogoutState));
            OnPropertyChanged(nameof(loginPopupState));
            OnPropertyChanged(nameof(EditSettingsVisible));
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
        public bool EditSettingsVisible
        {
            get => editSettingsVisible;
            set
            {
                editSettingsVisible = value;
                OnPropertyChanged();
            }
        }

        public bool signupPopupState => openSignupBool;
        public bool loginPopupState => openLoginBool;
        public bool settingsPopupState => openSettingsBool;
        public bool userLoginState => isUserLoggedOut;
        public bool userLogoutState => isUserLoggedIn;
       
        #endregion

        public void LogOut()
        {
            if (App.LoggedInUser == null)

                return;

            App.LoggedInUser = null;
            UserLoggedInCommand.Execute(null);
            _dialogService.ShowMessageAsync("Message", "You're logged out");

        }
    }
}
