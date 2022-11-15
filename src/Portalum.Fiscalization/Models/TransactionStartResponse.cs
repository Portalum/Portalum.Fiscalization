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
        public int SQ { get; set; }

        [JsonPropertyName("Result")]
        public TempResult Result { get; set; }

        [JsonPropertyName("Fis")]
        public TempFis Fis { get; set; }
    }

    public class TempResult
    {
        /// <summary>
        /// Result Code
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: RC</c>
        /// </remarks>
        [JsonPropertyName("RC")]
        public string ResultCode { get; set; }

        /// <summary>
        /// Warnings
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Warning</c>
        /// </remarks>
        [JsonPropertyName("Warning")]
        public string[] Warnings { get; set; }
    }

    public class TempFis
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
