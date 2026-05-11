using E_Kart_Application.DTOs.Orders;
using FluentValidation;

namespace E_Kart_Application.Validators.Orders
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {
            RuleFor(x => x.Freight)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Freight.HasValue);
        }
    }
}
