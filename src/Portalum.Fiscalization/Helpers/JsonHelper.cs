using System.Text.Json;

namespace Portalum.Fiscalization.Helpers
{
    public static class JsonHelper
    {
        public static string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);
            return JsonSerializer.Serialize(jsonElement, options);
        }
    }
}
