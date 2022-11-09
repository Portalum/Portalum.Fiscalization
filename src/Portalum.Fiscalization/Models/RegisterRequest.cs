using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class RegisterRequest
    {
        [JsonPropertyName("Tra")]
        public Transaction Transaction { get; set; }
    }
}
