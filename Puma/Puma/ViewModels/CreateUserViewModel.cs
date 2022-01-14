using Client.Models;
using Client.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class CreateUserViewModel : BaseViewModel
    {
        UserApiService _userApiService = new UserApiService();
        //public CreateUserViewModel(UserApiService userApiService) : base()
        //{
        //    _userApiService = userApiService;
        //    CreateUserCommand = new Command(CreateUser);
        //}
        public CreateUserViewModel()
        {
            CreateUserCommand = new Command(CreateUser);
        }
        public ICommand CreateUserCommand { get; }

        async void CreateUser()
        {
            var user = new UserDto()
            {
                Email = SignupEmail,
                Password = SignupPassword,
                DisplayName = SignupDisplayName,
                FirstName = SignupFirstName ?? "",
                LastName = SignupSurname ?? ""
            };


            var createdUser = await _userApiService.CreateUserAsync(user);
            if (createdUser != null)
                System.Diagnostics.Debug.WriteLine($"User created!: {createdUser.FirstName}, User lastname: {createdUser.LastName}");
        }

        string _signupEmail;
        string _signupPassword;
        string _signupDisplayName;
        string _signupFirstName;
        string _signupSurname;
        public string SignupEmail
        {
            get => _signupEmail;
            set
            {
                _signupEmail = value;
                OnPropertyChanged();
            }
        }
        public string SignupPassword
        {
            get => _signupPassword;
            set
            {
                _signupPassword = value;
                OnPropertyChanged();
            }
        }
        public string SignupDisplayName
        {
            get => _signupDisplayName;
            set
            {
                _signupDisplayName = value;
                OnPropertyChanged();
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

    }
}
