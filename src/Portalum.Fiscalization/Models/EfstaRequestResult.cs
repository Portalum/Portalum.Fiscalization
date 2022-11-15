using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class EfstaRequestResult
    {
        /// <summary>
        /// Result Code
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: RC</c>
        /// </remarks>
        [JsonPropertyName("RC")]
        public string ResultCode { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: ErrorCode</c>
        /// </remarks>
        public string ErrorCode { get; set; }

        /// <summary>
        /// User Message
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: UserMessage</c>
        /// </remarks>
        [JsonPropertyName("UserMessage")]
        public string UserMessage { get; set; }

        /// <summary>
        /// Warnings
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Warning</c>
        /// </remarks>
        [JsonPropertyName("Warning")]
        public string[] Warnings { get; set; }
    }
}
