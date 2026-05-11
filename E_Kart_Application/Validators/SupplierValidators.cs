using E_Kart_Application.DTOs;
using FluentValidation;

namespace E_Kart_Application.Validators;

public class CreateSupplierValidator : AbstractValidator<CreateSupplierDto>
{
    public CreateSupplierValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(40).WithMessage("Company name cannot exceed 40 characters")
            .MinimumLength(2).WithMessage("Company name must be at least 2 characters");

        RuleFor(x => x.Phone)
            .MaximumLength(24).WithMessage("Phone cannot exceed 24 characters")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Fax)
            .MaximumLength(24).WithMessage("Fax cannot exceed 24 characters")
            .When(x => !string.IsNullOrEmpty(x.Fax));

        RuleFor(x => x.PostalCode)
            .MaximumLength(10).WithMessage("Postal code cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.PostalCode));

        RuleFor(x => x.Country)
            .MaximumLength(15).WithMessage("Country cannot exceed 15 characters")
            .When(x => !string.IsNullOrEmpty(x.Country));
    }
}

public class UpdateSupplierValidator : AbstractValidator<UpdateSupplierDto>
{
    public UpdateSupplierValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(40).WithMessage("Company name cannot exceed 40 characters")
            .MinimumLength(2).WithMessage("Company name must be at least 2 characters");

        RuleFor(x => x.Phone)
            .MaximumLength(24).WithMessage("Phone cannot exceed 24 characters")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Fax)
            .MaximumLength(24).WithMessage("Fax cannot exceed 24 characters")
            .When(x => !string.IsNullOrEmpty(x.Fax));

        RuleFor(x => x.PostalCode)
            .MaximumLength(10).WithMessage("Postal code cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.PostalCode));

        RuleFor(x => x.Country)
            .MaximumLength(15).WithMessage("Country cannot exceed 15 characters")
            .When(x => !string.IsNullOrEmpty(x.Country));
    }
}

public class PatchSupplierContactValidator : AbstractValidator<PatchSupplierContactDto>
{
    public PatchSupplierContactValidator()
    {
        RuleFor(x => x.ContactName)
            .MaximumLength(30).WithMessage("Contact name cannot exceed 30 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactName));

        RuleFor(x => x.ContactTitle)
            .MaximumLength(30).WithMessage("Contact title cannot exceed 30 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactTitle));

        RuleFor(x => x.Phone)
            .MaximumLength(24).WithMessage("Phone cannot exceed 24 characters")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Fax)
            .MaximumLength(24).WithMessage("Fax cannot exceed 24 characters")
            .When(x => !string.IsNullOrEmpty(x.Fax));
    }
}

public class PatchSupplierAddressValidator : AbstractValidator<PatchSupplierAddressDto>
{
    public PatchSupplierAddressValidator()
    {
        RuleFor(x => x.Address)
            .MaximumLength(60).WithMessage("Address cannot exceed 60 characters")
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.City)
            .MaximumLength(15).WithMessage("City cannot exceed 15 characters")
            .When(x => !string.IsNullOrEmpty(x.City));

        RuleFor(x => x.PostalCode)
            .MaximumLength(10).WithMessage("Postal code cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.PostalCode));

        RuleFor(x => x.Country)
            .MaximumLength(15).WithMessage("Country cannot exceed 15 characters")
            .When(x => !string.IsNullOrEmpty(x.Country));
    }
}
