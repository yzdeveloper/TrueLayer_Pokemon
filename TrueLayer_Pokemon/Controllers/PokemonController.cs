using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return Ok(await _pokemonService.Get(name));
        }
    }
}
