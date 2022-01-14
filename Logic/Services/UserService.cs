using Microsoft.EntityFrameworkCore;
using PumaDbLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Models;
using AutoMapper;

namespace Logic.Services
{
    public class UserService
    {
        private PumaDbContext _context;
        private IMapper _mapper;

        public UserService(PumaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> LogIn(string email, string password)
        {
            var foundUser = _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower() && u.Password == EncryptionService.Encrypt(password));
            if (foundUser == null)
                return null;

            return _mapper.Map<UserDto>(foundUser);
        }

        public async Task<ICollection<UserDto>> GetAllAsync()
        {
            var dbUsers = await _context.Users.ToListAsync();
            return _mapper.Map<ICollection<UserDto>>(dbUsers);
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            User dbUser = await GetDbUserAsync(id);

            if (dbUser == null)
                return null;

            return _mapper.Map<UserDto>(dbUser);
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            var dbUser = await GetDbUserAsync(email);

            if (dbUser == null)
                return null;

            return _mapper.Map<UserDto>(dbUser);
        }

        public async Task<UserDto> PostUserAsync(UserDto newUser)
        {
            if (!IsNewUserRequestValid(newUser))
                return null;            

            var foundUser = await GetDbUserAsync(newUser.Email);

            if (foundUser != null)
                return _mapper.Map<UserDto>(foundUser);

            User userToAdd = _mapper.Map<User>(newUser);
            userToAdd.IsActive = true;
            userToAdd.Password = EncryptionService.Encrypt(newUser.Password);

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

        public async Task<UserDto> EditUser(UserDto user)
        {
            var userToEdit = await GetDbUserAsync(user.Id);

            if (userToEdit == null)
                return null;

            //EditUser(user, userToEdit);

            userToEdit = _mapper.Map<User>(user);

            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(userToEdit);
        }

        private async Task<User> GetDbUserAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        private async Task<User> GetDbUserAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());
        }

        //public async Task<UserDto> SetAdmin(int id, bool value)
        //{
        //    var userToEdit = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        //    if (userToEdit == null)
        //        return null;

        //    EditUserBoolProperty(userToEdit, "IsAdmin", value);

        //    await _context.SaveChangesAsync();
        //    return _mapper.Map<UserDto>(userToEdit);
        //}

        //private static void EditUserBoolProperty(User userToEdit, string property, bool value)
        //{
        //    switch (property.ToLower())
        //    {
        //        case "isactive":
        //            userToEdit.IsActive = value;
        //            break;

        //        case "isadmin":
        //            userToEdit.IsAdmin = value;
        //            break;

        //        default:
        //            break;
        //    }
        //}

        private static bool IsNewUserRequestValid(UserDto newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.Email))
                return false;

            return true;
        }
    }
}
