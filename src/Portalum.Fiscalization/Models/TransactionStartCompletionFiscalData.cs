using System;
using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionStartCompletionFiscalData
    {
        /// <summary>
        /// TransactionId
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TID</c>
        /// </remarks>
        [JsonPropertyName("TID")]
        public int TransactionId { get; set; }

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
