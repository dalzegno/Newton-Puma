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

        public async Task<UserDto> GetUserAsync(int userId)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (dbUser == null)
                return null;

            return UserTranslator.ToModel(dbUser);
        }

        public async Task<UserDto> PostUserAsync(UserDto newUser)
        {
            if (!IsNewUserRequestValid(newUser))
                return null;

            var foundUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == newUser.Email);

            if (foundUser != null)
                return UserTranslator.ToModel(foundUser);

            User userToAdd = new()
            {
                Email = newUser.Email,
                Comments = new List<Comment>(),
                DisplayName = newUser.DisplayName ?? "",
                FirstName = newUser.FirstName ?? "",
                LastName = newUser.LastName ?? "",
                Gradings = new List<Grading>(),
                Password = newUser.Password
            };

            try
            {
                await _context.Users.AddAsync(userToAdd);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //throw new Exception("Något gick fel vid sparande till databasen.");
                return null;
            }

            return UserTranslator.ToModel(userToAdd);
        }

        private static bool IsNewUserRequestValid(UserDto newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.Email))
                return false;

            return true;
        }
    }
}
