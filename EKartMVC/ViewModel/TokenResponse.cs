using System.Text.Json.Serialization;

namespace EKartMVC.Models
{
    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }

    }
}
