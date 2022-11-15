using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class PaymentElement
    {
        /// <summary>
        /// Type
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: _</c>
        /// </remarks>
        [JsonPropertyName("_")]
        public string Type { get; } = "Pay";

        /// <summary>
        /// Description
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Dsc</c>
        /// </remarks>
        [JsonPropertyName("Dsc")]
        public string Description { get; set; }

        /// <summary>
        /// Payment Amount, Amount paid
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Amt</c>
        /// </remarks>
        [JsonPropertyName("Amt")]
        public string PaymentAmount { get; set; }

        /// <summary>
        /// Unique Identifier, Unique payment identifier (electronic payment or document reference)
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: UID</c>
        /// </remarks>
        [JsonPropertyName("UID")]
        public string UniqueIdentifier { get; set; }
    }
}