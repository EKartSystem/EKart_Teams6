using AutoMapper;
using E_Kart_Application.DTOs.CategoryDto;
using E_Kart_Application.DTOs.EmployeeDto;
using E_Kart_Application.DTOs.LocationDTO;
using E_Kart_Application.Models;

namespace E_Kart_Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ReverseMap();

            CreateMap<Category, CreateCategoryDto>()
                .ReverseMap();

            CreateMap<Category, UpdateCategoryDto>()
                .ReverseMap();

            CreateMap<Category, UpdateCategoryNameDto>()
                .ReverseMap();

            CreateMap<Category, UpdateCategoryDescriptionDto>()
                .ReverseMap();

            CreateMap<Product, ProductListingDto>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.CategoryName)
                )

                .ForMember(
                    dest => dest.IsInStock,
                    opt => opt.MapFrom(src => src.UnitsInStock > 0)
                )

                .ReverseMap();

            CreateMap<Category, CategoryWithProductCountDto>()
                .ForMember(
                    dest => dest.ProductCount,
                    opt => opt.MapFrom(src => src.Products.Count)
                );
            //employee
            CreateMap<Employee, EmployeeDto>()
     .ReverseMap();
            CreateMap<Territory,TerritoryDto>().ReverseMap();
            CreateMap<Employee, CreateEmployeeDto>()
                .ReverseMap();

            CreateMap<Employee, UpdateEmployeeDto>()
                .ReverseMap();

            CreateMap<Employee, UpdateEmployeeTitleDto>()
                .ReverseMap();

            CreateMap<Employee, EmployeeListingDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src =>
                        src.FirstName + " " + src.LastName)
                )

                .ForMember(
                    dest => dest.IsManager,
                    opt => opt.MapFrom(src =>
                        src.InverseReportsToNavigation.Any())
                )

                .ForMember(
                    dest => dest.TotalEmployeesUnderManager,
                    opt => opt.MapFrom(src =>
                        src.InverseReportsToNavigation.Count)
                )

                .ReverseMap();

        }
    }
}