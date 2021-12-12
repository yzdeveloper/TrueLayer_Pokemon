using System.Text.Json;

namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Parser for the translation API
    /// </summary>
    public class TranslationParser : ITranslationParser
    {
        /// <summary>
        /// Parse the transaltion service response into translation result
        /// </summary>
        /// <param name="json">the translation service response</param>
        /// <returns>translation</returns>
        public string Parse(string json)
        {
            var jDoc = JsonDocument.Parse(json);
            if (jDoc.RootElement.TryGetProperty("contents", out JsonElement contents))
            {
                if (contents.TryGetProperty("translated", out JsonElement translated))
                {
                    return translated.GetString();
                }
            }

            return null;
        }
    }


}