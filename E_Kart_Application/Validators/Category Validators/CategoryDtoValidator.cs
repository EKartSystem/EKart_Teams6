using E_Kart_Application.DTOs.CategoryDto;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Category Id must be greater than 0");

            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithMessage("Category Name is required")
                .MaximumLength(100)
                .WithMessage("Category Name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.ProductCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product Count cannot be negative");
        }
    }
}