using Puma.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Puma.Services
{
    public interface IUserApiService
    {
        Task<User> LogIn(string email, string password);
        Task<User> GetUserAsync(string email);
        Task<List<User>> GetAll();
        Task<User> CreateUserAsync(AddUserDto userToCreate);
        Task<User> UpdateUserAsync(UpdateUserDto userToUpdate);
        Task<User> DeleteUserAsync(int id);

    }
}