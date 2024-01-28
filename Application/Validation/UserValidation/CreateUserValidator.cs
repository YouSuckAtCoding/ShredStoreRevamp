using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using FluentValidation;

namespace Application.Validation.UserValidation
{
    public class CreateUserValidator : AbstractValidator<User>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please insert a name");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Please insert a address");
            RuleFor(x => x.Cpf).NotEmpty().Length(11).WithMessage("Please insert a valid cpf");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please insert a vali email address");
            RuleFor(x => x.Age).NotEmpty().LessThan(110).GreaterThan(16).WithMessage("Please insert a valid age (16 - 110)");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("Please insert a valid password (more than 8 digits)");
        }
    }
}
