using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class RegistrationResult
    {
        /// <summary>
        /// Result Code
        /// </summary>
        [JsonPropertyName("RC")]
        public string ResultCode { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// User Message
        /// </summary>
        public string UserMessage { get; set; }

        /// <summary>
        /// Warnings
        /// </summary>
        [JsonPropertyName("Warning")]
        public string[] Warnings { get; set; }
    }
}
