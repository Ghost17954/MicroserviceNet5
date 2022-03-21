using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator:AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} should not be empty ")
                .NotNull().WithMessage("{UserName} should not be null ")
                .MaximumLength(50).WithMessage("{UserName} should not exceed a length of 15");

            RuleFor(p => p.EmailAddress).NotEmpty().WithMessage("{EmailAddress} can be empty");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} can be empty")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greated than zero");
        }
    }
}
