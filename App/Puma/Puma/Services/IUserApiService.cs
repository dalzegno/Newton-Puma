using Puma.Models;
using System;
using System.Threading.Tasks;

namespace Puma.Services
{
    public interface IUserApiService
    {
        Task<User> LogIn(string email, string password);
        Task<User> GetUserAsync(string email);
        Task<User> CreateUserAsync(User userToCreate);
        Task<User> UpdateUserAsync(User userToUpdate);
        Task<User> DeleteUserAsync(string email);
        Task<User> GetCurrentUserAsync(string email, User currentUser);

    }
}