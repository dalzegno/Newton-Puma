using FluentValidation;
using Puma.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace Puma.Services
{
    internal class UserValidationService : AbstractValidator<UserDto>
    {
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public UserValidationService()
        {
            RuleFor(User => User.DisplayName).MinimumLength(4).WithMessage("Minimum length 4");

            RuleFor(User => User.Password).MinimumLength(6).WithMessage("Minimum length 6");

            RuleFor(User => User.Email).Matches(emailRegex).WithMessage("A valid email is required");
        }

    }
}
