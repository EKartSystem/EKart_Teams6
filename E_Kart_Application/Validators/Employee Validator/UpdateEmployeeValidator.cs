using E_Kart_Application.DTOs.EmployeeDto;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class UpdateEmployeeValidator
        : AbstractValidator<ResponseEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(20);

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotEmpty();

            RuleFor(x => x.HomePhone)
                .NotEmpty()
                .Matches(@"^[0-9]{10}$")
                .WithMessage(
                    "Phone number must be 10 digits");

            RuleFor(x => x.ReportsTo)
                .GreaterThan(0)
                .When(x => x.ReportsTo != null);
        }
    }
}