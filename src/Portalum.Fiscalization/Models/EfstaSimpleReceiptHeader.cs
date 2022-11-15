using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class EfstaSimpleReceiptHeader
    {
        /// <summary>
        /// Type
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: _</c>
        /// </remarks>
        [JsonPropertyName("_")]
        public string Type { get; } = "Txt";


        /// <summary>
        /// Text
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: value</c>
        /// </remarks>
        [JsonPropertyName("value")]
        public string Text { get; set; }
    }
}
