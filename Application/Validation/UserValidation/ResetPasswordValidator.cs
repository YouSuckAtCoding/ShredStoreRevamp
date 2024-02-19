using Contracts.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation.UserValidation
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordUserRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8).WithMessage("Please insert a valid password (more than 8 digits)");
        }
    }
}
