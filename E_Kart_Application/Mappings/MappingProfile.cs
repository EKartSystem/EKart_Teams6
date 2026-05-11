using AutoMapper;
using E_Kart_Application.DTOs.OrderDetails;
using E_Kart_Application.DTOs.Orders;
using E_Kart_Application.Models;

namespace E_Kart_Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();
            CreateMap<UpdateOrderAddressDto, Order>();
            CreateMap<UpdateOrderStatusDto, Order>();
            CreateMap<UpdateOrderShipperDto, Order>();
            CreateMap<OrderDetail, OrderDetailResponseDto>()
                .ForMember(
                    dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.ProductName)
                );
            CreateMap<UpdateOrderDetailDto, OrderDetail>();
        }
    }
}