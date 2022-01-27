﻿using Puma.Models;
using Puma.Services;
using System.Windows.Input;
using Xamarin.Forms;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Puma.ViewModels
{
    public class NewUserViewModel : BaseViewModel
    {
        readonly IUserApiService _userApiService;
        readonly IDialogService _dialogService;
        public NewUserViewModel(IUserApiService userApiService, IDialogService dialogService)
        {
            _userApiService = userApiService;
            _dialogService = dialogService;
            //CreateUserCommand = new Command(CreateUser);

            //_userApiService.ErrorMessage += ReportErrorMessage;
        }

        Command _createUserCommand;
        public Command CreateUserCommand => _createUserCommand ?? (_createUserCommand = new Command(CreateUser, CanCreate));

        string _signupEmail;
        string _signupPassword;
        string _signupDisplayName;
        string _signupFirstName;
        string _signupSurname;

        string _emailErrorMSG;
        string _passwordErrorMSG;
        string _displayNameErrorMSG;

        public string EmailErrorMSG
        {
            get => _emailErrorMSG;
            set
            {
                _emailErrorMSG = value;
                OnPropertyChanged();
                CreateUserCommand.ChangeCanExecute();
            }
        }
        public string PasswordErrorMSG
        {
            get => _passwordErrorMSG;
            set
            {
                _passwordErrorMSG = value;
                OnPropertyChanged();
                CreateUserCommand.ChangeCanExecute();
            }
        }
        public string DisplayNameErrorMSG
        {
            get => _displayNameErrorMSG ;
            set
            {
                _displayNameErrorMSG = value;
                OnPropertyChanged();
                CreateUserCommand.ChangeCanExecute();
                
            }
        }
        public string SignupEmail
        {
            get => _signupEmail;
            set
            {
                _signupEmail = value;
                OnPropertyChanged();
                CreateUserCommand.ChangeCanExecute();

            }
        }
        public string SignupPassword
        {
            get => _signupPassword;
            set
            {
                _signupPassword = value;
                OnPropertyChanged();
                CreateUserCommand.ChangeCanExecute();
            }
        }
        public string SignupDisplayName
        {
            get => _signupDisplayName;
            set
            {
                _signupDisplayName = value;
                OnPropertyChanged();
                CreateUserCommand.ChangeCanExecute();
            }
        }
        public string SignupFirstName
        {
            get => _signupFirstName;
            set
            {
                _signupFirstName = value;
                OnPropertyChanged();
            }
        }
        public string SignupSurname
        {
            get => _signupSurname;
            set
            {
                _signupSurname = value;
                OnPropertyChanged();
            }
        }

        private bool CanCreate() => !string.IsNullOrWhiteSpace(SignupEmail) && !string.IsNullOrWhiteSpace(SignupPassword) && !string.IsNullOrWhiteSpace(SignupDisplayName);

        public bool Uservalidation(AddUserDto user)
        {
            UserValidationService validationRules = new UserValidationService();
            ValidationResult ans = validationRules.Validate(user);

            Dictionary<string, string> errorPropertyName = new Dictionary<string, string>();

            if (ans == null || !ans.IsValid)
            {
                foreach (var item in ans.Errors)
                {

                    errorPropertyName.Add(item.PropertyName, item.ErrorMessage);

                }

                if (errorPropertyName.ContainsKey("Email")) 
                    EmailErrorMSG = errorPropertyName["Email"];
                else 
                    EmailErrorMSG = "";

                if (errorPropertyName.ContainsKey("Password")) 
                    PasswordErrorMSG = errorPropertyName["Password"];
                else 
                    PasswordErrorMSG = "";

                if (errorPropertyName.ContainsKey("DisplayName")) 
                    DisplayNameErrorMSG = errorPropertyName["DisplayName"];
                else 
                    DisplayNameErrorMSG = "";

                return false;
            }

            EmailErrorMSG = "";
            PasswordErrorMSG = "";
            DisplayNameErrorMSG = "";

            return true;

        }
        private async void CreateUser()
        {
            var user = new AddUserDto()
            {
                Email = SignupEmail,
                Password = SignupPassword,
                DisplayName = SignupDisplayName,
                FirstName = SignupFirstName ?? "",
                LastName = SignupSurname ?? ""
            };

            if (Uservalidation(user))
            {
                var createdUser = await _userApiService.CreateUserAsync(user);

                if (createdUser != null)
                {
                    await _dialogService.ShowMessageAsync("Welcome!", $"Welcome to PUMA \"{createdUser.DisplayName}\".");
                    return;
                }
            }
        }

        private async void ReportErrorMessage(object sender, string message) => await _dialogService.ShowMessageAsync("Error", $"{message}");

    }
}
