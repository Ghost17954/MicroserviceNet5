using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries
{
    public class GetOrderListQueryValidator:AbstractValidator<GetOrderListQuery>
    {
        public GetOrderListQueryValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} should not be empty ")
                .NotNull().WithMessage("{UserName} should not be null ")
                .MaximumLength(50).WithMessage("{UserName} should not exceed a length of 50");
        }
    }
}
