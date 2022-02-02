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

        public Command EditUserCommand => _editUserCommand ?? (_editUserCommand = new Command(EditUser, CanEdit));

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


        public void SetUserToEdit(string displayName, string email, string firstName, string lastName)
        {
            if (App.LoggedInUser == null)
                return;
            CurrentUserDisplayName = displayName;
            CurrentUserFirstName = firstName;
            CurrentUserLastName = lastName;
            CurrentUserEmail = email;
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
        private bool CanEdit() => !string.IsNullOrWhiteSpace(EditEmail) && !string.IsNullOrWhiteSpace(EditPassword) && !string.IsNullOrWhiteSpace(EditDisplayName);
        private async void EditUser()
        {
            var user = new UpdateUserDto()
            {
                Id = App.LoggedInUser.Id,
                Email = EditEmail,
                Password = EditPassword,
                DisplayName = EditDisplayName,
                FirstName = EditFirstName ?? "",
                LastName = EditSurname ?? ""
            };

            var updatedUser = await _userApiService.UpdateUserAsync(user);
            if (updatedUser != null)
            {
                await _dialogService.ShowMessageAsync("Saved!!", $"Settings applied! \"{user.DisplayName}\".");
            }
        }
    }
}
