using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class PositionElementLine : PositionElementBase
    {
        /// <inheritdoc/>
        [JsonPropertyName("_")]
        public override string Type { get; } = "Lin";

        /// <summary>
        /// Line Amount
        /// </summary>
        /// <remarks>
        /// Text in amount column, can show subtotal amount
        /// </remarks>
        [JsonPropertyName("LAmt")]
        public string LineAmount { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("Dsc")]
        public string Description { get; set; }
    }
}