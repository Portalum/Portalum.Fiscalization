using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class EfstaSimpleReceiptFooter
    {
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("_")]
        public string Type { get; } = "Txt";


        [JsonPropertyName("value")]
        public string Text { get; set; }
    }
}
