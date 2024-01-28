using Contracts.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation.UserValidation
{
    public class LoginValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please insert a vali email address");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("Please insert a valid password (more than 8 digits)");
        }
    }
}

