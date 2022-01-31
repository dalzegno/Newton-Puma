using Puma.Services;
using Xamarin.Forms;
using Puma.Models;
using Puma.Views;

namespace Puma.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private IUserApiService _userApiService;
        private IDialogService _dialogService;

        public LoginViewModel(IUserApiService userApiService,
                              IDialogService dialogService)
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
        public bool IsNotLoggedIn => StaticUser.LoggedInUser == null;

        public async void LogIn()
        {
            System.Diagnostics.Debug.Write(LoginEmail + "\n" + LoginPassword);
            //var loggedInUser = await _userApiService.LogIn(LoginEmail, LoginPassword);

            //if (loggedInUser == null)
            //{
            //    await _dialogService.ShowErrorAsync("Login", $"Mail or password was wrong.", "OK");
            //    return;
            //}

            //StaticUser.LoggedInUser = loggedInUser;
            //await _dialogService.ShowMessageAsync("Login", $"Welcome {loggedInUser.DisplayName}!");
            //MainPage.Instance.MainViewModel.UserLoggedInCommand.Execute(null);
        }
    }
}
