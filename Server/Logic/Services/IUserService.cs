using Logic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IUserService
    {
        Task<UserDto> LogInAsync(string email, string password);
        Task<ICollection<UserDto>> GetAllAsync();
        Task<UserDto> GetAsync(int id);
        Task<UserDto> GetAsync(string email);
        Task<UserDto> PostAsync(UserDto newUser);
        Task<UserDto> EditAsync(UserDto user);
        Task<UserDto> DeleteAsync(int id);

    }
}