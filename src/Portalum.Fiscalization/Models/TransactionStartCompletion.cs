using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionStartCompletion
    {
        /// <summary>
        /// Sequence Number
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: SQ</c>
        /// </remarks>
        [JsonPropertyName("SQ")]
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Result</c>
        /// </remarks>
        [JsonPropertyName("Result")]
        public EfstaRequestResult Result { get; set; }

        /// <summary>
        /// Fiscal Data, fiscal Signature
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Fis</c>
        /// </remarks>
        [JsonPropertyName("Fis")]
        public TransactionStartCompletionFiscalData FiscalData { get; set; }
    }
}
