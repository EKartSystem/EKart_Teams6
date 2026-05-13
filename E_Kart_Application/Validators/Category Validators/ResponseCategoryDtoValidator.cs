using E_Kart_Application.DTOs.CategoryDto;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class ResponseCategoryDtoValidator : AbstractValidator<ResponseCategoryDto>
    {
        public ResponseCategoryDtoValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithMessage("Category Name is required")
                .MaximumLength(100)
                .WithMessage("Category Name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters");
        }
    }
}