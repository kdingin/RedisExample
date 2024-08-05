using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validation
{
    public class ProductDtoValidation : AbstractValidator<ProductDto>
    {
        public ProductDtoValidation()
        {
            RuleFor(x=> x.Name).NotEmpty().WithMessage(("{PropertyName} is required")).NotNull().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Price).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater than 0");
            RuleFor(x => x.Stock).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater than 0");
            RuleFor(x => x.categoryId).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater than 0");
        }
    }
}
