using E_Kart_Application.DTOs.LocationDTO;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class AddTerritoryValidator : AbstractValidator<TerritoryDto>
    {
        public AddTerritoryValidator()
        {
            RuleFor(x => x.TerritoryId).NotNull();
            RuleFor(x => x.RegionId).NotNull().GreaterThan(0).LessThan(5);
            RuleFor(x => x.TerritoryDescription).NotEmpty();
        }
    }
}
