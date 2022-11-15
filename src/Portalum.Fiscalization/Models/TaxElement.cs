using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TaxElement
    {
        /// <summary>
        /// Type
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: _</c>
        /// </remarks>
        [JsonPropertyName("_")]
        public string Type { get; } = "Tax";

        /// <summary>
        /// Tax Group
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TaxG</c>
        /// </remarks>
        [JsonPropertyName("TaxG")]
        public string TaxGroup { get; set; }

        /// <summary>
        /// Tax Percent
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Prc</c>
        /// </remarks>
        [JsonPropertyName("Prc")]
        public string TaxPercent { get; set; }

        /// <summary>
        /// Amount excluding Tax
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Net</c>
        /// </remarks>
        [JsonPropertyName("Net")]
        public string NetAmount { get; set; }

        /// <summary>
        /// Tax Amount
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TAmt</c>
        /// </remarks>
        [JsonPropertyName("TAmt")]
        public string TaxAmount { get; set; }

        /// <summary>
        /// Amount including Tax
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Amt</c>
        /// </remarks>
        [JsonPropertyName("Amt")]
        public string GrossAmount { get; set; }
    }
}