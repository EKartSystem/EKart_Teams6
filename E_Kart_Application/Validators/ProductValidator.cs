using E_Kart_Application.DTOs.ProductsDTO;
using FluentValidation;

namespace E_Kart_Application.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.UnitPrice).NotNull().GreaterThan(0);
            RuleFor(x => x.QuantityPerUnit).NotEmpty();
            RuleFor(x => x.CategoryId).NotNull().GreaterThan(0);
            RuleFor(x => x.SupplierId).NotNull().GreaterThan(0);
        }
    }
}
