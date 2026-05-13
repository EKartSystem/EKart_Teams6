using E_Kart_Application.DTOs.CategoryDto;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class ProductListingDtoValidator : AbstractValidator<ProductListingDto>
    {
        public ProductListingDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product Id must be greater than 0");

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product Name is required")
                .MaximumLength(100)
                .WithMessage("Product Name cannot exceed 100 characters");

            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithMessage("Category Name is required")
                .MaximumLength(100)
                .WithMessage("Category Name cannot exceed 100 characters");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.UnitPrice.HasValue)
                .WithMessage("Unit Price cannot be negative");

            RuleFor(x => x.QuantityPerUnit)
                .MaximumLength(50)
                .WithMessage("Quantity Per Unit cannot exceed 50 characters");
        }
    }
}