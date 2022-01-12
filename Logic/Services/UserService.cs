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

        public ICollection<UserDto> Get()
        {
            var dbUsers = _context.Users;
            List<UserDto> userResults = new List<UserDto>();

            foreach (var dbUser in dbUsers)
            {
                userResults.Add(UserTranslator.ToModel(dbUser));
            }

            return userResults;
        }

        public async Task<UserDto> PostUserAsync(UserDto newUser)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == newUser.Id);

            if (foundUser != null)
                return UserTranslator.ToModel(foundUser);

            User userToAdd = new()
            {
                Id = newUser.Id,
                Comments = new List<Comment>(),
                DisplayName = newUser.DisplayName ?? "",
                FirstName = newUser.FirstName ?? "",
                LastName = newUser.LastName ?? "",
                Email = newUser.Email,
                Gradings = new List<Grading>(),
                Password = newUser.Password
            };

            await _context.Users.AddAsync(userToAdd);
            await _context.SaveChangesAsync();
            return UserTranslator.ToModel(userToAdd);
        }
    }
}
