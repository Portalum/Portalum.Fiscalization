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
        /// <remarks>
        /// <c>Efsta EFR Field: PN</c>
        /// </remarks>
        [JsonPropertyName("PN")]
        public string PositionNumber { get; set; }

        /// <summary>
        /// Item Number, public article number (GTIN, EAN, UPC)
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: IN</c>
        /// </remarks>
        [JsonPropertyName("IN")]
        public string ItemNumber { get; set; }

        /// <summary>
        /// Item Identity, serial or batch number
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: ID</c>
        /// </remarks>
        [JsonPropertyName("ID")]
        public string ItemIdentity { get; set; }

        /// <summary>
        /// Stock Keeping Unit, article number
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: SKU</c>
        /// </remarks>
        [JsonPropertyName("SKU")]
        public string StockKeepingUnit { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Dsc</c>
        /// </remarks>
        [JsonPropertyName("Dsc")]
        public string Description { get; set; }

        /// <summary>
        /// Tax Group
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TaxG</c>
        /// </remarks>
        [JsonPropertyName("TaxG")]
        public string TaxGroup { get; set; }

        /// <summary>
        /// Position Amount
        /// <para>(Qty*Pri)</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Amt</c>
        /// </remarks>
        [JsonPropertyName("Amt")]
        public string PositionAmount { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Qty</c>
        /// </remarks>
        [JsonPropertyName("Qty")]
        public string Quantity { get; set; }

        /// <summary>
        /// Quantity Unit
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: QtyU</c>
        /// </remarks>
        [JsonPropertyName("QtyU")]
        public string QuantityUnit { get; set; }

        /// <summary>
        /// Unit Price
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Pri</c>
        /// </remarks>
        [JsonPropertyName("Pri")]
        public string UnitPrice { get; set; }

        //public string LAmt { get; set; }
    }
}