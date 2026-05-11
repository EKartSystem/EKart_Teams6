namespace E_Kart_Application.DTOs.EmployeeDto
{
    public class EmployeeListingDto
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? Title { get; set; }

        public bool IsManager { get; set; }

        public int TotalEmployeesUnderManager { get; set; }
    }
}
