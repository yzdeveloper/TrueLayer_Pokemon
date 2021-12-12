using System.Linq;
using System.Text.Json;

namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Pokemon service response parser
    /// </summary>
    public class PokemonParser : IPokemonParser
    {
        /// <summary>
        /// Parse pokemon from json string 
        /// </summary>
        /// <param name="result">json string</param>
        /// <returns>Pokemon object</returns>
        public Pokemon ToPokemon(string result)
        {
            var jDoc = JsonDocument.Parse(result);

            return new Pokemon
            {
                Name = GetName(jDoc),
                Description = GetDescription(jDoc),
                Habitat = GetHabitat(jDoc),
                IsLegendary = GetLegendary(jDoc)
            };
        }

        #region Private

        private static bool GetLegendary(JsonDocument jDoc)
        {
            return jDoc.RootElement.TryGetProperty("is_legendary", out JsonElement l) && l.GetBoolean();
        }

        private static string GetHabitat(JsonDocument jDoc)
        {
            if (jDoc.RootElement.TryGetProperty("habitat", out JsonElement h))
            {
                if (h.ValueKind == JsonValueKind.Object && h.TryGetProperty("name", out JsonElement n))
                {
                    return n.GetString();
                }
            }

            return null;
        }

        private static string GetDescription(JsonDocument jDoc)
        {
            if (jDoc.RootElement.TryGetProperty("flavor_text_entries", out JsonElement entries))
            {
                return entries.EnumerateArray().Where(e => 
                    {
                        if (e.TryGetProperty("language", out JsonElement lan))
                        {
                            if (lan.TryGetProperty("name", out JsonElement n))
                            {
                                return n.GetString() == "en";
                            }
                        }

                        return false;
                    }).Select(e => 
                        {
                            if (e.TryGetProperty("flavor_text", out JsonElement t))
                            {
                                return t.GetString();
                            }

                            return null;
                        }).FirstOrDefault();
            }

            return null;
        }

        private static string GetName(JsonDocument jDoc)
        {
            jDoc.RootElement.TryGetProperty("name", out JsonElement name);
            return name.GetString();
        }

        #endregion
    }
}
