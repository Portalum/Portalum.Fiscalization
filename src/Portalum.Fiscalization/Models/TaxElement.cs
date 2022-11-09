using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TaxElement
    {
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("_")]
        public string Type { get; } = "Tax";

        /// <summary>
        /// Tax Group
        /// </summary>
        [JsonPropertyName("TaxG")]
        public string TaxGroup { get; set; }

        /// <summary>
        /// Tax Percent
        /// </summary>
        [JsonPropertyName("Prc")]
        public string TaxPercent { get; set; }

        /// <summary>
        /// Excluding Tax
        /// </summary>
        [JsonPropertyName("Net")]
        public string Net { get; set; }

        /// <summary>
        /// Tax Amount
        /// </summary>
        [JsonPropertyName("TAmt")]
        public string TaxAmount { get; set; }

        /// <summary>
        /// Including Tax
        /// </summary>
        [JsonPropertyName("Amt")]
        public string GrossAmount { get; set; }
    }
}