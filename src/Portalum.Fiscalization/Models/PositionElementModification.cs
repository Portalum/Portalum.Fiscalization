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
        /// <remarks>
        /// <c>Efsta EFR Field: PN</c>
        /// </remarks>
        [JsonPropertyName("PN")]
        public string PositionNumber { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Dsc</c>
        /// </remarks>
        [JsonPropertyName("Dsc")]
        public string Description { get; set; }

        /// <summary>
        /// Position Amount
        /// <para>(Qty*Pri)</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Amt</c>
        /// </remarks>
        [JsonPropertyName("Amt")]
        public string PositionAmount { get; set; }
    }
}