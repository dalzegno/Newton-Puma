﻿using Logic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(int id);
        Task<UserDto> GetAsync(string email);
        Task<UserDto> LogInAsync(string email, string password);
        Task<ICollection<UserDto>> GetAllAsync();
        Task<bool> IsUserAuthorizedAsync(string apiKey);
        Task<UserDto> CreateAsync(AddUserDto newUser);
        Task<UserDto> UpdateAsync(AddUserDto user);
        Task<UserDto> DeleteAsync(int id);

    }
}