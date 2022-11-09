using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionCompletion
    {
        /// <summary>
        /// Sequence Number
        /// </summary>
        [JsonPropertyName("SQ")]
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Registration Result
        /// </summary>
        public RegistrationResult Result { get; set; }

        /// <summary>
        /// Fiscal Data
        /// </summary>
        [JsonPropertyName("Fis")]
        public FiscalData FiscalData { get; set; }
    }
}