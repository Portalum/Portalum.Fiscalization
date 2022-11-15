using System;
using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionStartResponse
    {
        [JsonPropertyName("TraSC")]
        public Temp1 Temp { get; set; }
    }

    public class Temp1
    {
        /// <summary>
        /// Sequence Number
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: SQ</c>
        /// </remarks>
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
        public TempFiscalData Fis { get; set; }
    }

    public class TempFiscalData
    {
        public int TID { get; set; }

        /// <summary>
        /// Operation start
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: StartD</c>
        /// </remarks>
        [JsonPropertyName("StartD")]
        public DateTime OperationStart { get; set; }
    }
}
