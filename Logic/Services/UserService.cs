using Microsoft.EntityFrameworkCore;
using PumaDbLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Translators;

namespace Logic.Services
{
    public class UserService
    {
        private PumaDbContext _context;

        public UserService(PumaDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<UserDto>> GetAsync()
        {
            var dbUsers = await _context.Users.ToListAsync();
            List<UserDto> userResults = new();

            foreach (var dbUser in dbUsers)
            {
                userResults.Add(UserTranslator.ToModel(dbUser));
            }

            return userResults;
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (dbUser == null)
                return null;

            return UserTranslator.ToModel(dbUser);
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (dbUser == null)
                return null;

            return UserTranslator.ToModel(dbUser);
        }

        public async Task<UserDto> PostUserAsync(UserDto newUser)
        {
            if (!IsNewUserRequestValid(newUser))
                return null;

            var foundUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == newUser.Email.ToLower());

            if (foundUser != null)
                return UserTranslator.ToModel(foundUser);

            User userToAdd = UserTranslator.ToEntity(newUser);
            userToAdd.IsActive = true;

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

            return UserTranslator.ToModel(userToAdd);
        }

        public async Task<UserDto> SetActive(int id, bool value)
        {
            var userToEdit = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if (userToEdit == null)
                return null;

            EditUserBoolProperty(userToEdit, "IsActive", value);

            await _context.SaveChangesAsync();
            return UserTranslator.ToModel(userToEdit);
        }

        public async Task<UserDto> SetAdmin(int id, bool value)
        {
            var userToEdit = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userToEdit == null)
                return null;

            EditUserBoolProperty(userToEdit, "IsAdmin", value);

            await _context.SaveChangesAsync();
            return UserTranslator.ToModel(userToEdit);
        }

        private static void EditUserBoolProperty(User userToEdit, string property, bool value)
        {
            switch (property.ToLower())
            {
                case "isactive":
                    userToEdit.IsActive = value;
                    break;

                case "isadmin":
                    userToEdit.IsAdmin = value;
                    break;

                default:
                    break;
            }
        }

        private static bool IsNewUserRequestValid(UserDto newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.Email))
                return false;

            return true;
        }
    }
}
