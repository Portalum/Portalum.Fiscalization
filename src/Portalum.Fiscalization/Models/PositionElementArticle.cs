using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class PositionElementArticle : PositionElementBase
    {
        /// <inheritdoc/>
        [JsonPropertyName("_")]
        public override string Type { get; } = "Pos";

        /// <summary>
        /// Position Number
        /// </summary>
        [JsonPropertyName("PN")]
        public string PositionNumber { get; set; }

        /// <summary>
        /// Item Number
        /// </summary>
        /// <remarks>
        /// Public article number (GTIN, EAN, UPC)
        /// </remarks>
        [JsonPropertyName("IN")]
        public string ItemNumber { get; set; }

        /// <summary>
        /// Item Identity
        /// </summary>
        /// <remarks>
        /// Serial or batch number
        /// </remarks>
        [JsonPropertyName("ID")]
        public string ItemIdentity { get; set; }

        /// <summary>
        /// Stock Keeping Unit
        /// </summary>
        /// <remarks>
        /// Article number
        /// </remarks>
        [JsonPropertyName("SKU")]
        public string StockKeepingUnit { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("Dsc")]
        public string Description { get; set; }

        /// <summary>
        /// Tax Group
        /// </summary>
        [JsonPropertyName("TaxG")]
        public string TaxGroup { get; set; }

        /// <summary>
        /// Position Amount
        /// </summary>
        /// <remarks>
        /// (Qty*Pri)
        /// </remarks>
        [JsonPropertyName("Amt")]
        public string PositionAmount { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("Qty")]
        public string Quantity { get; set; }

        /// <summary>
        /// Quantity Unit
        /// </summary>
        [JsonPropertyName("QtyU")]
        public string QuantityUnit { get; set; }

        /// <summary>
        /// Unit Price
        /// </summary>
        [JsonPropertyName("Pri")]
        public string UnitPrice { get; set; }

        //public string LAmt { get; set; }
    }
}