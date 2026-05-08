using E_Kart_Application.DTOs.Customersdto;
using FluentValidation;

public class LoginValidator : AbstractValidator<CustomerLogin>
{
    public LoginValidator()
    {
      RuleFor(x=>x.ContactName).NotEmpty(); 

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}