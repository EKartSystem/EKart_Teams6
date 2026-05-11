using E_Kart_Application.DTOs;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class CreateShipperValidator : AbstractValidator<CreateShipperDto>
    {
        public CreateShipperValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(40).WithMessage("Company name cannot exceed 40 characters")
                .MinimumLength(2).WithMessage("Company name must be at least 2 characters");

            RuleFor(x => x.Phone)
                .MaximumLength(24).WithMessage("Phone number cannot exceed 24 characters")
                .Matches(@"^[\d\s\(\)\-\+]*$")
                .WithMessage("Phone number can only contain digits, spaces, brackets, dashes and +")
                .When(x => !string.IsNullOrEmpty(x.Phone)); // only validate if phone is provided
        }
    }

    public class UpdateShipperValidator : AbstractValidator<UpdateShipperDto>
    {
        public UpdateShipperValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(40).WithMessage("Company name cannot exceed 40 characters")
                .MinimumLength(2).WithMessage("Company name must be at least 2 characters");

            RuleFor(x => x.Phone)
                .MaximumLength(24).WithMessage("Phone number cannot exceed 24 characters")
                .Matches(@"^[\d\s\(\)\-\+]*$")
                .WithMessage("Phone number can only contain digits, spaces, brackets, dashes and +")
                .When(x => !string.IsNullOrEmpty(x.Phone));
        }
    }

    // Validator for PATCH name endpoint
    public class PatchShipperNameValidator : AbstractValidator<PatchShipperNameDto>
    {
        public PatchShipperNameValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(40).WithMessage("Company name cannot exceed 40 characters")
                .MinimumLength(2).WithMessage("Company name must be at least 2 characters");
        }
    }

    public class PatchShipperPhoneValidator : AbstractValidator<PatchShipperPhoneDto>
    {
        public PatchShipperPhoneValidator()
        {
            RuleFor(x => x.Phone)
                .MaximumLength(24).WithMessage("Phone number cannot exceed 24 characters")
                .Matches(@"^[\d\s\(\)\-\+]*$")
                .WithMessage("Phone number can only contain digits, spaces, brackets, dashes and +")
                .When(x => !string.IsNullOrEmpty(x.Phone));
        }
    }
}
