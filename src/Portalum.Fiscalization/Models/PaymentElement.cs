using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class PaymentElement
    {
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("_")]
        public string Type { get; } = "Pay";

        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("Dsc")]
        public string Description { get; set; }

        /// <summary>
        /// Payment Amount
        /// </summary>
        /// <remarks>
        /// Amount paid
        /// </remarks>
        [JsonPropertyName("Amt")]
        public string PaymentAmount { get; set; }

        /// <summary>
        /// Unique Identifier
        /// </summary>
        /// <remarks>
        /// Unique payment identifier (electronic payment or document reference)
        /// </remarks>
        [JsonPropertyName("UID")]
        public string UniqueIdentifier { get; set; }
    }
}