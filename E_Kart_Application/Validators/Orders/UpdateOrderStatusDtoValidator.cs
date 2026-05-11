using E_Kart_Application.DTOs.Orders;
using FluentValidation;

namespace E_Kart_Application.Validators.Orders
{
    public class UpdateOrderStatusDtoValidator : AbstractValidator<UpdateOrderStatusDto>
    {
        public UpdateOrderStatusDtoValidator()
        {
            RuleFor(x => x.ShippedDate).NotNull();
        }
    }
}
