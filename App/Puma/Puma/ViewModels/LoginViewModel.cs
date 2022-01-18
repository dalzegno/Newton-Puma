using Client.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private IUserApiService _userApiService;
        private IDialogService _dialogService;

        public LoginViewModel(IUserApiService userApiService, IDialogService dialogService)
        {
            _userApiService = userApiService;
            _dialogService = dialogService;
        }

        Command _logInCommand;
        public Command LoginCommand => _logInCommand ?? (_logInCommand = new Command(LogIn, CanLogIn));

        string _loginEmail;
        string _loginPassword;

        public string LoginEmail
        {
            get => _loginEmail;
            set
            {
                _loginEmail = value;
                OnPropertyChanged();
                LoginCommand.ChangeCanExecute();
            }
        }

        public string LoginPassword
        {
            get => _loginPassword;
            set
            {
                _loginPassword = value;
                OnPropertyChanged();
                LoginCommand.ChangeCanExecute();
            }
        }

        private bool CanLogIn() => !string.IsNullOrWhiteSpace(LoginEmail) && !string.IsNullOrWhiteSpace(LoginPassword);

        public async void LogIn()
        {
            var loggedInUser = await _userApiService.LogIn(LoginEmail, LoginPassword);
            if (loggedInUser != null)
            {
                await _dialogService.ShowMessageAsync("Login", $"Loggar in användare {loggedInUser.DisplayName}");
                return;
            }

            await _dialogService.ShowErrorAsync("Login", $"Mail eller lösenord fel.", "OK");
        }
    }
}
