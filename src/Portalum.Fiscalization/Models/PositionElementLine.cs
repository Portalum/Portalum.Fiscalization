using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class PositionElementLine : PositionElementBase
    {
        /// <inheritdoc/>
        [JsonPropertyName("_")]
        public override string Type { get; } = "Lin";

        /// <summary>
        /// Line Amount, rext in amount column, can show subtotal amount
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: LAmt</c>
        /// </remarks>
        [JsonPropertyName("LAmt")]
        public string LineAmount { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Dsc</c>
        /// </remarks>
        [JsonPropertyName("Dsc")]
        public string Description { get; set; }
    }
}