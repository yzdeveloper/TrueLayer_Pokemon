using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TrueLayer_Pokemon.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonParser _pokemonParser;

        public PokemonService(IPokemonParser pokemonParser)
        {
            _pokemonParser = pokemonParser;
        }

        public async Task<Pokemon> Get(string name)
        {
            var webClient = new WebClient();
            try
            {

                var result = await webClient.DownloadStringTaskAsync($"https://pokeapi.co/api/v2/pokemon-species/{name}");
                return _pokemonParser.ToPokemon(result);
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse wr && wr.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }
    }
}
