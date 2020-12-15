using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.ViewModels.Catalog.Products
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Id is required");

            RuleFor(x => x.Price)
                .NotNull()
                .WithMessage("Price is required.");

            RuleFor(x => x.OriginalPrice)
                .NotNull()
                .WithMessage("Original price is required.");

            RuleFor(x => x.Stock)
                .NotNull()
                .WithMessage("Stock is required.");

            RuleFor(x => x.ViewCount)
                .NotNull()
                .WithMessage("View count is required.");
        }
    }
}