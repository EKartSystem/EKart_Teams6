namespace E_Kart_Application.DTOs.LocationDTO
{
    public class RegionDto
    {
        public int RegionId { get; set; }
        public string RegionDescription { get; set; }
        public List<TerritoryDto> Territories { get; set; } = new();
    }
}
