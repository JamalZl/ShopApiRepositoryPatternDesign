using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Api.Apps.Admin.DTOs
{
    public class CategoryPostDto
    {
        public string Name { get; set; }
    }

    public class CategoryPostDtoValidator : AbstractValidator<CategoryPostDto>
    {
        public CategoryPostDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(20).WithMessage("Name can not be more than 20 characters")
                .NotEmpty().WithMessage("Name is required");
        }
    }
}
