using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class PositionElementModification : PositionElementBase
    {
        /// <inheritdoc/>
        [JsonPropertyName("_")]
        public override string Type { get; } = "Mod";

        /// <summary>
        /// Position Number
        /// </summary>
        [JsonPropertyName("PN")]
        public string PositionNumber { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("Dsc")]
        public string Description { get; set; }

        /// <summary>
        /// Position Amount
        /// </summary>
        /// <remarks>
        /// (Qty*Pri)
        /// </remarks>
        [JsonPropertyName("Amt")]
        public string PositionAmount { get; set; }
    }
}