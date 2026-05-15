using E_Kart_Application.DTOs.CategoryDto;
using FluentValidation;

public class CreateCategoryValidator : AbstractValidator<CategoryDto>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200);
    }
}