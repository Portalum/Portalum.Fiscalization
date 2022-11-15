using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class TransactionStartData
    {
        /// <summary>
        /// EFSTA Simple Receipt
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: ESR</c>
        /// </remarks>
        [JsonPropertyName("ESR")]
        public EfstaSimpleReceipt EfstaSimpleReceipt { get; set; }
    }
}
