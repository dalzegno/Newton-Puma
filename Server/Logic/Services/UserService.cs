using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using PumaDbLibrary;
using Logic.Models;
using AutoMapper;
using PumaDbLibrary.Entities;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private PumaDbContext _context;
        private IMapper _mapper;

        public UserService(PumaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> LogInAsync(string email, string password)
        {
            string encryptedPassword = EncryptionHelper.Encrypt(password);

            User foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower() && u.Password == encryptedPassword);

            if (foundUser == null)
                return null;

            return _mapper.Map<UserDto>(foundUser);
        }

        public async Task<ICollection<UserDto>> GetAllAsync()
        {
            var dbUsers = await _context.Users.ToListAsync();
            return _mapper.Map<ICollection<UserDto>>(dbUsers);
        }

        public async Task<UserDto> GetAsync(int id)
        {
            User dbUser = await GetDbUserAsync(id);

            if (dbUser == null)
                return null;

            return _mapper.Map<UserDto>(dbUser);
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var dbUser = await GetDbUserAsync(email);

            if (dbUser == null)
                return null;

            return _mapper.Map<UserDto>(dbUser);
        }

        public async Task<UserDto> PostAsync(UserDto newUser)
        {
            if (!IsNewUserRequestValid(newUser))
                return null;

            var foundUser = await GetDbUserAsync(newUser.Email);

            if (foundUser != null)
                return _mapper.Map<UserDto>(foundUser);

            User userToAdd = _mapper.Map<User>(newUser);

            userToAdd.Password = EncryptionHelper.Encrypt(newUser.Password);

            try
            {
                await _context.Users.AddAsync(userToAdd);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                //throw new Exception("Något gick fel vid sparande till databasen.");
                return null;
            }

            return _mapper.Map<UserDto>(userToAdd);
        }

        public async Task<UserDto> EditAsync(UserDto user)
        {
            var userToEdit = await GetDbUserAsync(user.Id);

            if (userToEdit == null)
                return null;

            //EditUser(user, userToEdit);

            User editedUser = _mapper.Map<User>(user);

            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(editedUser);
        }

        private async Task<User> GetDbUserAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        private async Task<User> GetDbUserAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());
        }

        public async Task<UserDto> DeleteAsync(int id)
        {
            var userToDelete = await _context.Users.Include(u => u.Comments)
                                                   .Include(u => u.Gradings)
                                                   .FirstOrDefaultAsync(u => u.Id == id);

            if (userToDelete == null)
                return null;

            try
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // TODO: Error handling
                return null;
            }

            return _mapper.Map<UserDto>(userToDelete);
        }

        private static bool IsNewUserRequestValid(UserDto newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.Email))
                return false;

            return true;
        }
    }
}
