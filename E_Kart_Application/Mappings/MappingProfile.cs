using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.DTOs.Customersdto;

using E_Kart_Application.Models;
using System.Drawing.Drawing2D;

namespace E_Kart_Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           

            CreateMap<Customer, CustomerDto>().ForMember(
              dest => dest.TotalOrders,
              opt => opt.MapFrom(src => src.Orders.Count)).ReverseMap();

            CreateMap<Customer, RegisterCustomerDto>().ForMember(dest => dest.Password,
                opt => opt.Ignore())

            .ForMember(dest => dest.Role,
                opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Customer, UpdateCustomerDto>()
                .ReverseMap();

          
            CreateMap<Order, OrderDto>();
        }
    }
}