using E_Kart_Application.DTOs.Orders;
using FluentValidation;

namespace E_Kart_Application.Validators.Orders
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().Length(5);
            RuleFor(x => x.Freight).GreaterThanOrEqualTo(0).When(x => x.Freight.HasValue);
            RuleFor(x => x.ShipName).MaximumLength(40);
            RuleFor(x => x.ShipAddress).MaximumLength(60);
            RuleFor(x => x.ShipCity).MaximumLength(15);
            RuleFor(x => x.ShipRegion).MaximumLength(15);
            RuleFor(x => x.ShipPostalCode).MaximumLength(10);
            RuleFor(x => x.ShipCountry).MaximumLength(15);
            RuleFor(x => x.Products).NotEmpty()
                .WithMessage("At least one product is required.");
            RuleForEach(x => x.Products)
                .ChildRules(product =>
                {
                    product.RuleFor(p => p.ProductId).GreaterThan(0);
                    product.RuleFor(p => p.Quantity).GreaterThan((short)0);
                });
        }
    }
}