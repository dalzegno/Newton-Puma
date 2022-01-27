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

        #region Create
        public async Task<UserDto> CreateAsync(AddUserDto newUser)
        {
            if (!IsNewUserRequestValid(newUser))
                return null;

            var foundUser = await GetDbUserAsync(newUser.Email);

            if (foundUser != null)
                return _mapper.Map<UserDto>(foundUser);


            User userToAdd = _mapper.Map<User>(newUser);

            userToAdd.Password = EncryptionHelper.Encrypt(newUser.Password);
            userToAdd.ApiKey = Guid.NewGuid().ToString();

            await _context.Users.AddAsync(userToAdd);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(userToAdd);
        }
        #endregion

        #region Read
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
        #endregion

        #region Update
        public async Task<UserDto> UpdateAsync(AddUserDto user)
        {
            var userToEdit = await GetDbUserAsync(user.Email);

            if (userToEdit == null)
                return null;

            User editedUser = _mapper.Map<User>(user);

            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(editedUser);
        }
        #endregion

        #region Delete
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
        #endregion

        #region Methods
        private async Task<User> GetDbUserAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        private async Task<User> GetDbUserAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());
        }
        private static bool IsNewUserRequestValid(AddUserDto newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.Email))
                return false;

            return true;
        }
        public async Task<bool> IsUserAuthorizedAsync(string apiKey)
        {
            return await _context.Users.AnyAsync(u => u.ApiKey == apiKey);
        }
        #endregion

    }
}
