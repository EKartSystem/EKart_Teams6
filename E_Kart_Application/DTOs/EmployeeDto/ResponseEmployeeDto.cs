namespace E_Kart_Application.DTOs.EmployeeDto
{
    public class ResponseEmployeeDto
    {
       
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? Title { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? HomePhone { get; set; }

        public int? ReportsTo { get; set; }
    }
}
