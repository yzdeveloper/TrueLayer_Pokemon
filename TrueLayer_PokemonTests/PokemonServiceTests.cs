using Moq;
using System;
using System.Net;
using System.Text.Json;
using TrueLayer_Pokemon.Services;
using Xunit;

namespace TrueLayer_PokemonTests
{
    public class PokemonServiceTests
    {
        private static readonly Random _rnd = new();

        [Fact]
        public async void WhenGetWithRandomName_ThenNull()
        {
            // Given 
            var mockParser = new Mock<IPokemonParser>();
            var target = new PokemonService(mockParser.Object);
            var name = Guid.NewGuid().ToString();

            // When
            var result = await target.Get(name);

            // Then
            Assert.Null(result);
            mockParser.Verify(p => p.ToPokemon(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async void WhenGetWithKnownName_ThenNotNull()
        {
            // Given 
            var target = new PokemonService(new PokemonParser());
            var name = "wormadam";

            // When
            var result = await target.Get(name);

            // Then
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.False(result.IsLegendary);
            Assert.Null(result.Habitat);
            Assert.NotNull(result.Description);
        }

        [Fact]
        public async void WhenGetWithKnownName_ThenAsParsed()
        {
            // Given 
            var mockParser = new Mock<IPokemonParser>();
            var pokemon = new Pokemon
            {
                Description = Guid.NewGuid().ToString(),
                Habitat = Guid.NewGuid().ToString(),
                IsLegendary = _rnd.Next() % 2 == 0,
                Name = Guid.NewGuid().ToString()
            };
            mockParser.Setup(p => p.ToPokemon(It.IsAny<string>())).Returns(pokemon);

            var target = new PokemonService(mockParser.Object);
            var name = "wormadam";

            // When
            var result = await target.Get(name);

            // Then
            Assert.NotNull(result);
            Assert.Equal(pokemon.Name, result.Name);
            Assert.Equal(pokemon.IsLegendary, result.IsLegendary);
            Assert.Equal(pokemon.Habitat, result.Habitat);
            Assert.Equal(pokemon.Description, result.Description);
        }

        //[Fact]
        internal async void Find_Legendary()
        {
            var service = new PokemonService(new PokemonParser());
            using var wc = new WebClient();
            var url = "https://pokeapi.co/api/v2/pokemon-species";

            do
            {
                var result = await wc.DownloadStringTaskAsync(url);
                var doc = JsonDocument.Parse(result);
                url = doc.RootElement.GetProperty("next").GetString();
                foreach (var r in doc.RootElement.GetProperty("results").EnumerateArray())
                {
                    var p = await service.Get(r.GetProperty("name").GetString());
                    if (p.IsLegendary)
                    {
                        // found
                    }
                }

            } while (url != null);
        }
    }
}
