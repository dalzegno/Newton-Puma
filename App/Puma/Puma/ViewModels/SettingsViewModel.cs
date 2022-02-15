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
using FluentValidation.Results;

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
        public Command EditUserCommand => _editUserCommand ?? (_editUserCommand = new Command(EditUser));
        public Command DeleteUserCommand => _deleteUserCommand ?? (_deleteUserCommand = new Command(DeleteUser));

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


        string _emailErrorMSG;
        string _passwordErrorMSG;
        string _displayNameErrorMSG;


        string _loginEmail;
        public string LoginEmail
        {
            get => _loginEmail;
            set
            {
                _loginEmail = value;
                OnPropertyChanged();

            }
        }


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


        public string EmailErrorMSG
        {
            get => _emailErrorMSG;
            set
            {
                _emailErrorMSG = value;
                OnPropertyChanged();
            }
        }
        public string PasswordErrorMSG
        {
            get => _passwordErrorMSG;
            set
            {
                _passwordErrorMSG = value;
                OnPropertyChanged();
            }
        }
        public string DisplayNameErrorMSG
        {
            get => _displayNameErrorMSG;
            set
            {
                _displayNameErrorMSG = value;
                OnPropertyChanged();

            }
        }
        //private bool CanEdit() => !string.IsNullOrWhiteSpace(EditPassword);

        public bool IsUserValid(UpdateUserDto user)
        {
            EditUserValidationService validationRules = new EditUserValidationService();
            ValidationResult validationResult = validationRules.Validate(user);

            Dictionary<string, string> errrorDictionary = new Dictionary<string, string>();

            EmailErrorMSG = "";
            PasswordErrorMSG = "";
            DisplayNameErrorMSG = "";

            if (validationResult.IsValid)
            {
                return true;
            }

            foreach (var item in validationResult.Errors)
            {
                errrorDictionary.Add(item.PropertyName, item.ErrorMessage);
            }

            AddErrorMessageToPropertyIfInvalid(errrorDictionary);

            return false;
        }

        private async void EditUser()
        {
            string passwordAnswer = await App.Current.MainPage.DisplayPromptAsync("Enter your current password please", "Password:");

            if (string.IsNullOrWhiteSpace(passwordAnswer))
                return;

            var authorizeUser = await _userApiService.LogIn(App.LoggedInUser.Email, passwordAnswer);
            if (authorizeUser == null)
                return;

            var user = new UpdateUserDto()
            {
                Id = App.LoggedInUser.Id,
                Email = EditEmail ?? App.LoggedInUser.Email,
                Password = EditPassword,
                DisplayName = EditDisplayName ?? App.LoggedInUser.DisplayName,
                FirstName = EditFirstName ?? App.LoggedInUser.FirstName,
                LastName = EditSurname ?? App.LoggedInUser.LastName,
            };

            if (!IsUserValid(user))
            {
                string errorMessages = "";
                errorMessages = AppendErrorMessages(errorMessages);
                await _dialogService.ShowErrorAsync("Error: " + errorMessages);
                return;
            }

            var updatedUser = await _userApiService.UpdateUserAsync(user);
            if (updatedUser != null)
            {

                await _dialogService.ShowMessageAsync("Saved!!", $"Settings applied! \"{user.DisplayName}\".");
                App.LoggedInUser = updatedUser;
                return;

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
                    string passwordAnswer = await App.Current.MainPage.DisplayPromptAsync("Enter your password please", "Password:");
                    if (string.IsNullOrWhiteSpace(passwordAnswer))
                        return;

                    var authorizeUser = await _userApiService.LogIn(App.LoggedInUser.Email, passwordAnswer);
                    if (authorizeUser == null)
                        return;

                    await _dialogService.ShowMessageAsync($"Deleted: {App.LoggedInUser.DisplayName}", "User has been deleted.");
                    await _userApiService.DeleteUserAsync(App.LoggedInUser.Id);
                    MainPage.Instance.MainViewModel.ClosePopupCommand.Execute(null);
                    MainPage.Instance.MainViewModel.LogOutCommand.Execute(null);
                    break;
            }
        }

        private void AddErrorMessageToPropertyIfInvalid(Dictionary<string, string> errorPropertyName)
        {
            if (errorPropertyName.ContainsKey("Email"))
                EmailErrorMSG = errorPropertyName["Email"];

            if (errorPropertyName.ContainsKey("Password"))
                PasswordErrorMSG = errorPropertyName["Password"];

            if (errorPropertyName.ContainsKey("DisplayName"))
                DisplayNameErrorMSG = errorPropertyName["DisplayName"];
        }
        private string AppendErrorMessages(string errorMessages)
        {
            if (!string.IsNullOrWhiteSpace(EmailErrorMSG))
                errorMessages += "Email: " + EmailErrorMSG + "\n";
            if (!string.IsNullOrWhiteSpace(PasswordErrorMSG))
                errorMessages += "Password: " + PasswordErrorMSG + "\n";
            if (!string.IsNullOrWhiteSpace(DisplayNameErrorMSG))
                errorMessages += "DisplayName: " + DisplayNameErrorMSG + "\n";
            return errorMessages;
        }
    }
}

