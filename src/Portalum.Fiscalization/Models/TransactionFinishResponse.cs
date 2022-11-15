using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionFinishResponse
    {
        /// <summary>
        /// Transaction Completion
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TraC</c>
        /// </remarks>
        [JsonPropertyName("TraC")]
        public TransactionFinishCompletion TransactionCompletion { get; set; }
    }
}
