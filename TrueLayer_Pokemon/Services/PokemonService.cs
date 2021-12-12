using System.Threading.Tasks;

namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Pokemon service
    /// </summary>
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonParser _pokemonParser;
        private readonly ITranslationService _translationService;
        private readonly IHttpService _httpService;

        public PokemonService(IPokemonParser pokemonParser, ITranslationService translationService,
            IHttpService httpService)
        {
            _pokemonParser = pokemonParser;
            _translationService = translationService;
            _httpService = httpService;
        }

        #region IPokemonService

        /// <summary>
        /// Get pokemon object from name
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>pokemon object</returns>
        public async Task<Pokemon> Get(string name)
        {
            var result = await _httpService.Get($"https://pokeapi.co/api/v2/pokemon-species/{name}");
            return _pokemonParser.ToPokemon(result);
        }


        /// <summary>
        /// Get pokemon object from name, Description of the pokemon will be translated
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>pokemon object</returns>
        public async Task<Pokemon> GetTranslated(string name)
        {
            var pokemon = await Get(name);
            return await _translationService.Translate(pokemon);
        }

        #endregion
    }
}
