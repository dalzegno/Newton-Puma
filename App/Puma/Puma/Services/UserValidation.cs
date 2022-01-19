using FluentValidation;
using Puma.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Puma.Services
{
    internal class UserValidation: AbstractValidator<UserDto>
    {
        Regex emailRegex = new Regex(@"^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");
        public UserValidation()
        {
            RuleFor(User => User.DisplayName).NotEmpty().WithMessage("Required").MinimumLength(4).WithMessage("Minimum length 4");
            RuleFor(User => User.FirstName).NotEmpty().WithMessage("Required").MinimumLength(4).WithMessage("Minimum length 4");
            RuleFor(User => User.LastName).NotEmpty().WithMessage("Required").MinimumLength(4).WithMessage("Minimum length 4");
            RuleFor(User => User.Password).NotEmpty().WithMessage("Required").MinimumLength(6).WithMessage("Minimum length 6");

            RuleFor(User => User.Email).NotEmpty().EmailAddress().Matches(emailRegex).WithMessage("A valid email is required");
        }
    }
}
