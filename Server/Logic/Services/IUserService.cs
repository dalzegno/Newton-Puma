using Logic.Models;
using PumaDbLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IUserService
    {
        Task<UserDto> LogIn(string email, string password);
        Task<ICollection<UserDto>> GetAllAsync();
        Task<UserDto> GetUserAsync(int id);
        Task<UserDto> GetUserAsync(string email);
        Task<UserDto> PostUserAsync(UserDto newUser);
        Task<UserDto> EditUserAsync(UserDto user);
        Task<UserDto> DeleteUser(UserDto user);

    }
}