using FluentValidation;
using E_Kart_Application.DTOs.OrderDetails;
namespace E_Kart_Application.Validators.OrderDetails
{
    public class UpdateOrderDetailDtoValidator : AbstractValidator<UpdateOrderDetailDto>
    {
        public UpdateOrderDetailDtoValidator()
        {
            RuleFor(x => x.UnitPrice).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan((short)0);
            RuleFor(x => x.Discount).InclusiveBetween(0, 1);
        }
    }
}