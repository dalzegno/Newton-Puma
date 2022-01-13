using Logic.Models;
using PumaDbLibrary;
using System.Collections.Generic;

namespace Logic.Translators
{
    public class UserTranslator
    {
        public static UserDto ToModel(User user)
        {
            if (user == null)
                return null;

            return new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                Email = user.Email.ToLower(),
                Password = user.Password
            };
        }

        public static User ToEntity(UserDto user)
        {
            if (user == null)
                return null;

            return new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                Email = user.Email.ToLower(),
                Password = user.Password,
                Comments = new List<Comment>(),
                Gradings = new List<Grading>()
            };
        }
    }
}
