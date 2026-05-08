using FluentValidation;
using E_Kart_Application.DTOs.Customersdto;
public class CustomerUpdateValidator : AbstractValidator<UpdateCustomerDto>
{
    public CustomerUpdateValidator()
    {
      
        RuleFor(x => x.ContactName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.ContactName))
            .WithMessage("Contact Name cannot exceed 50 characters");

       

        RuleFor(x => x.Address)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Address));

        //RuleFor(x => x.City)
        //    .MaximumLength(50)
        //    .When(x => !string.IsNullOrEmpty(x.City));

        //RuleFor(x => x.Region)
        //    .MaximumLength(50)
        //    .When(x => !string.IsNullOrEmpty(x.Region));

        //RuleFor(x => x.PostalCode)
        //    .MaximumLength(20)
        //    .When(x => !string.IsNullOrEmpty(x.PostalCode));

       

        RuleFor(x => x.Phone)
            .Matches(@"^[0-9]{10}$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Phone must be exactly 10 digits");

        
    }
}