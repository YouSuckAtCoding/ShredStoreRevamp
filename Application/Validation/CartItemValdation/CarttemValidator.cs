using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation.CartItemValdation
{
    public class CarttemValidator : AbstractValidator<CartItem>
    {
        public CarttemValidator()
        {
            RuleFor(x => x.CartId).GreaterThan(0);
            RuleFor(x => x.ProductId).GreaterThan(0);            
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
