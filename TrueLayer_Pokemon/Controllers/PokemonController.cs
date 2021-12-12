using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueLayer_Pokemon.Services;

namespace TrueLayer_Pokemon.Controllers
{
    [Route("pokemon")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet()]
        [Route("{name}")]
        public async Task<IActionResult> GetPokemon(string name)
        {
            var found = await _pokemonService.Get(name);
            if (found == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(await _pokemonService.Get(name));
            }
        }

        [HttpGet()]
        [Route("translated/{name}")]
        public async Task<IActionResult> GetPokemonTranslated(string name)
        {
            var found = await _pokemonService.GetTranslated(name);
            if (found == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(found);
            }
        }

    }
}
