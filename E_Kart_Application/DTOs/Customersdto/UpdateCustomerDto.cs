using AutoMapper;
using E_Kart_Application.DTOs.Customersdto;
namespace E_Kart_Application.DTOs.Customersdto
{
    public class UpdateCustomerDto
    {
        public string? ContactName { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

    }
}
