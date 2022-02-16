using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Api.Apps.Admin.DTOs.CategoryDtos
{
    public class ProductPostDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool DisplayStatus { get; set; }
    }

    public class ProductPostDtoValidator : AbstractValidator<ProductPostDto>
    {
        public ProductPostDtoValidator()
        {
            RuleFor(x => x.CategoryId).NotNull().GreaterThanOrEqualTo(1);

            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage("Uzunluq max 50 ola biler , qaqa!")
                .NotEmpty().WithMessage("Name mecburidir!");

            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0).WithMessage("CostPrice 0-dan asagi ola bilmez!")
                .NotNull().WithMessage("CostPrice mecburidir!");

            RuleFor(x => x.SalePrice)
                .GreaterThanOrEqualTo(0).WithMessage("SalePrice 0-dan asagi ola bilmez!")
                .NotNull().WithMessage("SalePrice mecburidir!");

            RuleFor(x => x.DisplayStatus).NotNull().WithMessage("DisplayStatus mecburidir!");

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.CostPrice > x.SalePrice)
                    context.AddFailure("CostPrice", "CostPrice SalePrice-dan boyuk ola bilmez");
            });
        }
    }
}
