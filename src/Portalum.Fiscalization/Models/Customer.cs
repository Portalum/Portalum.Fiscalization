using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class Customer
    {
        /// <summary>
        /// Name
        /// </summary>
        /// <remarks>
        /// Customer or Company Name
        /// </remarks>
        [JsonPropertyName("Nam")]
        public string Name { get; set; }

        /// <summary>
        /// Name 2
        /// </summary>
        /// <remarks>
        /// Name Extension
        /// </remarks>
        [JsonPropertyName("Nam2")]
        public string Name2 { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        /// <remarks>
        /// Customer or Company Address within City 
        /// </remarks>
        [JsonPropertyName("Adr")]
        public string Address { get; set; }

        /// <summary>
        /// Address2
        /// </summary>
        /// <remarks>
        /// Address Extension
        /// </remarks>
        [JsonPropertyName("Adr2")]
        public string Address2 { get; set; }

        /// <summary>
        /// Postal Code
        /// </summary>
        /// <remarks>
        /// According to national rules without country prefix
        /// </remarks>
        [JsonPropertyName("Zip")]
        public string Zip { get; set; }

        /// <summary>
        /// City
        /// </summary>
        /// <remarks>
        /// City Name
        /// </remarks>
        [JsonPropertyName("City")]
        public string City { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        /// <remarks>
        /// Country ISO 3166 ALPHA-2
        /// </remarks>
        [JsonPropertyName("Ctry")]
        public string Country { get; set; }

        /// <summary>
        /// VAT Number
        /// </summary>
        /// <remarks>
        /// Customer or Company VAT Number including eventual country prefix
        /// </remarks>
        [JsonPropertyName("TaxId")]
        public string VatNumber { get; set; }

        /// <summary>
        /// Customer Number
        /// </summary>
        /// <remarks>
        /// Number of the Customer
        /// </remarks>
        [JsonPropertyName("CN")]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        /// <remarks>
        /// Category of the Customer
        /// </remarks>
        [JsonPropertyName("Cat")]
        public string Category { get; set; }
    }
}
