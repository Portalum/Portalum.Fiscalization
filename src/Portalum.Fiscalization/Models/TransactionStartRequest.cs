using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionStartRequest
    {
        [JsonPropertyName("TraS")]
        public Temp Temp { get; set; }
    }

    public class Temp
    {
        /// <summary>
        /// EFSTA Simple Receipt
        /// </summary>
        [JsonPropertyName("ESR")]
        public EfstaSimpleReceipt EfstaSimpleReceipt { get; set; }
    }
}
