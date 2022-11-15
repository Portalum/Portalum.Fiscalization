using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class RegisterResponse
    {
        /// <summary>
        /// TransactionCompletion
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TraC</c>
        /// </remarks>
        [JsonPropertyName("TraC")]
        public TransactionCompletion TransactionCompletion { get; set; }
    }
}
