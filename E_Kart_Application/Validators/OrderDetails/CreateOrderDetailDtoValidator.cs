using FluentValidation;
using E_Kart_Application.DTOs.OrderDetails;
namespace E_Kart_Application.Validators.OrderDetails
{
    public class CreateOrderDetailDtoValidator : AbstractValidator<CreateOrderDetailDto>
    {
        public CreateOrderDetailDtoValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan((short)0);
        }
    }
}