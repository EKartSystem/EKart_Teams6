using E_Kart_Application.DTOs.Customersdto;
using FluentValidation;

namespace E_Kart_Application.Validators
{


    public class RegisterValidator : AbstractValidator<RegisterCustomerDto>
    {
        public RegisterValidator()
        {


            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .WithMessage("Company Name is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.ContactName).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Phone)
                .Matches(@"^[0-9]{10}$")
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("Phone must be exactly 10 digits");

            RuleFor(x => x.Country)
               .MaximumLength(15)
               .WithMessage("Country cannot exceed 15 characters");
            

            RuleFor(x => x.Role)
              .NotEmpty().WithMessage("Role is required")
               .Must(role => role == "Admin" || role == "Customer" )
               .WithMessage("Invalid role. Allowed roles are Admin, Customer");
        }
    }
    }