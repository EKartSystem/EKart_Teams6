using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.ShipperDto;
using FluentValidation;

namespace E_Kart_Application.Validators;

public class ShipperRequestValidator : AbstractValidator<ShipperRequestDto>
{
    public ShipperRequestValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MinimumLength(2).WithMessage("Company name must be at least 2 characters")
            .MaximumLength(40).WithMessage("Company name cannot exceed 40 characters");

        RuleFor(x => x.Phone)
            .MaximumLength(24).WithMessage("Phone cannot exceed 24 characters")
            .Matches(@"^[\d\s\(\)\-\+]*$")
            .WithMessage("Phone can only contain digits, spaces, brackets, dashes and +")
            .When(x => !string.IsNullOrEmpty(x.Phone));
    }
}

public class PatchShipperNameValidator : AbstractValidator<PatchShipperNameDto>
{
    public PatchShipperNameValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MinimumLength(2).WithMessage("Company name must be at least 2 characters")
            .MaximumLength(40).WithMessage("Company name cannot exceed 40 characters");
    }
}

public class PatchShipperPhoneValidator : AbstractValidator<PatchShipperPhoneDto>
{
    public PatchShipperPhoneValidator()
    {
        RuleFor(x => x.Phone)
            .MaximumLength(24).WithMessage("Phone cannot exceed 24 characters")
            .Matches(@"^[\d\s\(\)\-\+]*$")
            .WithMessage("Phone can only contain digits, spaces, brackets, dashes and +")
            .When(x => !string.IsNullOrEmpty(x.Phone));
    }
}