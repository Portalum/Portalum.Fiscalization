using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class Transaction
    {
        /// <summary>
        /// EFSTA Simple Receipt
        /// </summary>
        [JsonPropertyName("ESR")]
        public EfstaSimpleReceipt EfstaSimpleReceipt { get; set; }
    }
}
