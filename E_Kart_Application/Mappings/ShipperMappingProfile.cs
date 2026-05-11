using AutoMapper;
using E_Kart_Application.DTOs;
using E_Kart_Application.Models;

namespace E_Kart_Application.Mappings
{
    // AutoMapper reads this file to know how to convert between
    // the raw database Model and the DTOs we send/receive.
    //
    // Think of it like a translator:
    //   Shipper (DB model) <---> ShipperDto (what client sees)
    public class ShipperMappingProfile : Profile
    {
        public ShipperMappingProfile()
        {
            // Shipper (model) → ShipperDto (response)
            // AutoMapper auto-matches properties with the same name.
            CreateMap<Shipper, ShipperDto>();

            // CreateShipperDto (request body) → Shipper (model to save to DB)
            CreateMap<CreateShipperDto, Shipper>();

            // UpdateShipperDto (PUT body) → Shipper (model to update in DB)
            CreateMap<UpdateShipperDto, Shipper>();
        }
    }
}
