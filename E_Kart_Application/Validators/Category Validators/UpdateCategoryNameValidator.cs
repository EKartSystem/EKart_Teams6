using E_Kart_Application.DTOs.CategoryDto;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class UpdateCategoryNameValidator
        : AbstractValidator<UpdateCategoryNameDto>
    {
        public UpdateCategoryNameValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required")
                .MinimumLength(3)
                .MaximumLength(50);
        }
    }
}