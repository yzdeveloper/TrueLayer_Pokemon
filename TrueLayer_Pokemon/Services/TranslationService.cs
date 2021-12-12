using System;
using System.Threading.Tasks;

namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Translate pokemon description service
    /// </summary>
    public class TranslationService : ITranslationService
    {
        private readonly IHttpService _httpService;
        private readonly ITranslationParser _translataionParser;

        public TranslationService(IHttpService httpService, ITranslationParser translataionParser)
        {
            _httpService = httpService;
            _translataionParser = translataionParser;
        }

        /// <summary>
        /// Translate pokemon description if needed
        /// </summary>
        /// <param name="pokemon">The pokemon object untranslated</param>
        /// <returns>The pokemon object translated</returns>
        public async Task<Pokemon> Translate(Pokemon pokemon)
        {
            var pokemonNew = new Pokemon 
            { 
                Description = null, 
                Habitat = pokemon.Habitat, 
                IsLegendary = pokemon.IsLegendary,
                Name = pokemon.Name
            };

            try
            {
                pokemonNew.Description = _translataionParser.Parse(
                    await ((pokemon.IsLegendary || pokemon.Habitat == "cave")
                        ? TranslateYoda(pokemon.Description) :
                        TranslateShakespeare(pokemon.Description)));
            }
            catch (Exception)
            {
                // add log here
                pokemonNew.Description = pokemon.Description;
            }

            return pokemonNew;
        }

        #region Private

        private Task<string> TranslateShakespeare(string description)
        {
            return TranslateAny("shakespeare", description); ;
        }

        private Task<string> TranslateYoda(string description)
        {
            return TranslateAny("yoda", description); ;
        }

        private Task<string> TranslateAny(string style, string description)
        {
            return _httpService.Get($"https://api.funtranslations.com/translate/{style}.json?text={Encode(description)}");
        }

        private static string Encode(string text)
        {
            return Uri.EscapeDataString(text.Replace("\n", " "));
        }

        #endregion
    }
}
