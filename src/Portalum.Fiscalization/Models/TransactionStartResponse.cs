using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionStartResponse
    {
        /// <summary>
        /// Transaction Completion
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TraSC</c>
        /// </remarks>
        [JsonPropertyName("TraSC")]
        public TransactionStartCompletion TransactionCompletion { get; set; }
    }
}
