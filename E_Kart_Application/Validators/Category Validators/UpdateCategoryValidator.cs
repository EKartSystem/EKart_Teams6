using E_Kart_Application.DTOs.CategoryDto;
using FluentValidation;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
       

        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200);
    }
}