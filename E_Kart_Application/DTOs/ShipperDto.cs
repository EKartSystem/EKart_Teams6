namespace E_Kart_Application.DTOs
{
    // This is what we SEND BACK to the client as a response.
    // We never send the raw Model directly — always use a DTO.
    public class ShipperDto
    {
        public int ShipperId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }
}
