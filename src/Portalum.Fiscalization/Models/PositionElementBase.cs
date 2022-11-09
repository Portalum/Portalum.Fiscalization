using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    [JsonDerivedType(typeof(PositionElementArticle))]
    [JsonDerivedType(typeof(PositionElementLine))]
    public abstract class PositionElementBase
    {
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("_")]
        public abstract string Type { get; }
    }
}