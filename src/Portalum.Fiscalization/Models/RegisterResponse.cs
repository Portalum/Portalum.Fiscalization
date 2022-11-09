using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class RegisterResponse
    {
        [JsonPropertyName("TraC")]
        public TransactionCompletion TransactionCompletion { get; set; }
    }
}
