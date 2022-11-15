using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionStartRequest
    {
        /// <summary>
        /// Transaction
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TraS</c>
        /// </remarks>
        [JsonPropertyName("TraS")]
        public TransactionStartData Transaction { get; set; }
    }
}
