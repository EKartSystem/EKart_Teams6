namespace E_Kart_Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public int? EmployeeId { get; set; }
        public DateTime? RequiredDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
        public List<CreateOrderItemDto> Products { get; set; } = new List<CreateOrderItemDto>();

    }
}