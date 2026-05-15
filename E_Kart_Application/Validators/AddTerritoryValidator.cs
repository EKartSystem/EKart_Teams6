using E_Kart_Application.DTOs.LocationDTO;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class AddTerritoryValidator : AbstractValidator<TerritoryDto>
    {
        public AddTerritoryValidator()
        {
            RuleFor(x => x.TerritoryId)
                 .NotEmpty();

            RuleFor(x => x.TerritoryDescription)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.RegionId)
                .GreaterThan(0).LessThan(5);
        }
    }
}
