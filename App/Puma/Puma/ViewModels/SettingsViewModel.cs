using Puma.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Puma.Services;
using Puma.Models;
using Puma.Views;

namespace Puma.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        readonly IUserApiService _userApiService;
        readonly IDialogService _dialogService;
        public SettingsViewModel(IUserApiService userApiService, IDialogService dialogService)
        {
            _userApiService = userApiService;
            _dialogService = dialogService;

        }

        Command _editUserCommand;
        Command _deleteUserCommand;
        Command _logInCommand;
        // public Command LoginCommand => _logInCommand ?? (_logInCommand = new Command(LogIn));
        public Command EditUserCommand => _editUserCommand ?? (_editUserCommand = new Command(EditUser));
        public Command DeleteUserCommand => _deleteUserCommand ?? (_deleteUserCommand = new Command(DeleteUser));

        //Vill vi att man kan byta email?
        string _editEmail;
        string _editPassword;
        string _editDisplayName;
        string _editFirstName;
        string _editSurname;
        string _currentUserDisplayName;
        string _currentUserFirstName;
        string _currentUserLastName;
        string _currentUserEmail;
        string _currentUserPassword;


        string _loginEmail;
        //string _loginPassword;
        public string LoginEmail
        {
            get => _loginEmail;
            set
            {
                _loginEmail = value;
                OnPropertyChanged();
                //LoginCommand.ChangeCanExecute();
            }
        }

        /* public string LoginPassword
         {
             get => _loginPassword;
             set
             {
                 _loginPassword = value;
                 OnPropertyChanged();
                 LoginCommand.ChangeCanExecute();
             }
         }*/



        public void SetUserToEdit(string displayName, string email, string firstName, string lastName, string password)
        {
            if (App.LoggedInUser == null)
                return;
            

            CurrentUserDisplayName = displayName;
            CurrentUserFirstName = firstName;
            CurrentUserLastName = lastName;
            CurrentUserEmail = email;
            CurrentUserPassword = password;
        }

        public string CurrentUserDisplayName
        {
            get => _currentUserDisplayName;
            set
            {
                _currentUserDisplayName = value;
                OnPropertyChanged();
            }
        }
        public string CurrentUserFirstName
        {
            get => _currentUserFirstName;
            set
            {
                _currentUserFirstName = value;
                OnPropertyChanged();
            }
        }
        public string CurrentUserLastName
        {
            get => _currentUserLastName;
            set
            {
                _currentUserLastName = value;
                OnPropertyChanged();
            }
        }
        public string CurrentUserEmail
        {
            get => _currentUserEmail;
            set
            {
                _currentUserEmail = value;
                OnPropertyChanged();
            }
        }
        public string CurrentUserPassword
        {
            get => _currentUserPassword;
            set
            {
                _currentUserPassword = value;
                OnPropertyChanged();
            }
        }
        public string EditEmail
        {
            get => _editEmail;
            set
            {
                _editEmail = value;
                OnPropertyChanged();
                EditUserCommand.ChangeCanExecute();

            }
        }
        public string EditPassword
        {
            get => _editPassword;
            set
            {
                if (_editPassword == null)
                    return;

                _editPassword = value;
                OnPropertyChanged();
                EditUserCommand.ChangeCanExecute();
            }
        }
        public string EditDisplayName
        {
            get => _editDisplayName;
            set
            {
                _editDisplayName = value;
                OnPropertyChanged();
                EditUserCommand.ChangeCanExecute();
            }
        }
        public string EditFirstName
        {
            get => _editFirstName;
            set
            {
                _editFirstName = value;
                OnPropertyChanged();
            }
        }
        public string EditSurname
        {
            get => _editSurname;
            set
            {
                _editSurname = value;
                OnPropertyChanged();
            }
        }
        //private bool CanEdit() => !string.IsNullOrWhiteSpace(EditPassword);

        private async void EditUser()
        {
            string passwordAnswer = await App.Current.MainPage.DisplayPromptAsync("Enter your password please", "Password:");
            EditPassword = passwordAnswer;
            var authorizeUser = await _userApiService.LogIn(App.LoggedInUser.Email, passwordAnswer);
            if (authorizeUser == null)
            {
                await _dialogService.ShowMessageAsync($"User credentials didnt match user: {App.LoggedInUser.DisplayName}", "Try again.");
                return;
            }

            var user = new UpdateUserDto()
            {
                Id = App.LoggedInUser.Id,
                Email = EditEmail ?? App.LoggedInUser.Email,
                Password = EditPassword,
                DisplayName = EditDisplayName ?? App.LoggedInUser.DisplayName,
                FirstName = EditFirstName ?? App.LoggedInUser.FirstName,
                LastName = EditSurname ?? App.LoggedInUser.LastName,
            };

            //await _userApiService.UpdateUserAsync(user)
            var updatedUser = await _userApiService.UpdateUserAsync(user);
            if (updatedUser != null)
            {
                var confirmationPopup = await App.Current.MainPage.DisplayActionSheet($"Change Password of User aswell:  {App.LoggedInUser.DisplayName}?", "No",
                               "Yes");
                switch (confirmationPopup)
                {
                    case "No":
                        await _userApiService.UpdateUserAsync(user);
                        await _dialogService.ShowMessageAsync("Saved!!", $"Settings applied! \"{user.DisplayName}\".");
                        break;
                    case "Yes":
                        string changePw = await App.Current.MainPage.DisplayPromptAsync("Password change", "New Password:");
                        EditPassword = changePw;
                        await _dialogService.ShowMessageAsync($"Password Changed of user: {user.DisplayName}", "& Settings have been applied!.");
                        await _userApiService.UpdateUserAsync(user);
                        break;

                }
            }
        }

        private async void DeleteUser()
        {
            if (App.LoggedInUser == null)
                return;

            var confirmationPopup = await App.Current.MainPage.DisplayActionSheet($"Delete User {App.LoggedInUser.DisplayName}?", "No",
                 "Yes");
            switch (confirmationPopup)
            {
                case "No":
                    break;
                case "Yes":
                    await _dialogService.ShowMessageAsync($"Deleted: {App.LoggedInUser.DisplayName}", "User has been deleted.");
                    await _userApiService.DeleteUserAsync(App.LoggedInUser.Id);
                    break;
            }
        }

        /* public async void LogIn()
         {
             var loggedInUser = await _userApiService.LogIn(LoginEmail, LoginPassword);

             if (loggedInUser == null)
             {
                 await _dialogService.ShowMessageAsync($"Email or Password of user: {loggedInUser.DisplayName}", "was wrong, try again");
                 return;
             }

             App.LoggedInUser = loggedInUser;*/
    }
}

