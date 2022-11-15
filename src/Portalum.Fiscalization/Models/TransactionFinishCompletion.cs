using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionFinishCompletion
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
        public EfstaRequestResult Result { get; set; }

        /// <summary>
        /// Fiscal Data
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Fis</c>
        /// </remarks>
        [JsonPropertyName("Fis")]
        public TransactionFinishCompletionFiscalData FiscalData { get; set; }
    }
}