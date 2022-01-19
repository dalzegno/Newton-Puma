﻿using Puma.Models;
using System;
using System.Threading.Tasks;

namespace Puma.Services
{
    public interface IUserApiService
    {
        Task<UserDto> LogIn(string email, string password);
        Task<UserDto> GetUserAsync(string email);
        Task<UserDto> CreateUserAsync(UserDto userToCreate);
        Task<UserDto> UpdateUserAsync(UserDto userToUpdate);
        Task<UserDto> DeleteUserAsync(string email);
    }
}