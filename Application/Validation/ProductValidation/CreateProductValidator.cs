using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation.ProductValidation
{
    public class CreateProductValidator : AbstractValidator<Product>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please insert a name");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(300).MinimumLength(20).WithMessage("Please insert a description");
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0).WithMessage("Please insert a valid price");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Please insert the product type");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Please insert a category");
            RuleFor(x => x.Brand).NotEmpty().WithMessage("Please insert a brand name");

        }
    }
}
