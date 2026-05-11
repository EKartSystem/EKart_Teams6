namespace E_Kart_Application.DTOs.Customersdto
{
    public class CustomerLogin
    {

        public string? ContactName { get; set; }    
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}