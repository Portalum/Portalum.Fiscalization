using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class RegisterRequest
    {
        /// <summary>
        /// Transaction
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Tra</c>
        /// </remarks>
        [JsonPropertyName("Tra")]
        public Transaction Transaction { get; set; }
    }
}
