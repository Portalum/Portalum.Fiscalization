using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionFinishRequest
    {
        /// <summary>
        /// Transaction
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Tra</c>
        /// </remarks>
        [JsonPropertyName("Tra")]
        public TransactionFinishData Transaction { get; set; }
    }
}
