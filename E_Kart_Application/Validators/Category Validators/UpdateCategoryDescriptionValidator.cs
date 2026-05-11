using E_Kart_Application.DTOs.CategoryDto;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class UpdateCategoryDescriptionValidator
        : AbstractValidator<UpdateCategoryDescriptionDto>
    {
        public UpdateCategoryDescriptionValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(5)
                .MaximumLength(200);
        }
    }
}