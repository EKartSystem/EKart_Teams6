using E_Kart_Application.DTOs.EmployeeDto;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class UpdateEmployeeTitleValidator
        : AbstractValidator<UpdateEmployeeTitleDto>
    {
        public UpdateEmployeeTitleValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);
        }
    }
}