using Moq;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TrueLayer_Pokemon.Services;
using Xunit;

namespace TrueLayer_PokemonTests
{
    public class PokemonServiceTests
    {
        private static readonly Random _rnd = new();

        #region Get/Translated

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void WhenGet_ThenUrlGenerated(bool translated)
        {
            // Given
            var mockParser = new Mock<IPokemonParser>();
            var mockHttp = new Mock<IHttpService>();
            var mockTranslator = new Mock<ITranslationService>();
            var randomName = Guid.NewGuid().ToString();
            var expectedUrl = $@"https://pokeapi.co/api/v2/pokemon-species/{randomName}";
            var target = new PokemonService(mockParser.Object, mockTranslator.Object, mockHttp.Object);

            // When
            var method = translated ? target.GetTranslated : (Func<string, Task<Pokemon>>)target.Get;
            var res = await method(randomName);

            // Then
            mockHttp.Verify(h => h.Get(expectedUrl), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void WhenGet_ThenPokemonParserGetFromHttpServie(bool translated)
        {
            // Given
            var mockParser = new Mock<IPokemonParser>();
            var mockHttp = new Mock<IHttpService>();
            var expectedJson = Guid.NewGuid().ToString();
            mockHttp.Setup(h => h.Get(It.IsAny<string>())).Returns(Task.FromResult(expectedJson));
            var mockTranslator = new Mock<ITranslationService>();
            var randomName = Guid.NewGuid().ToString();
            var target = new PokemonService(mockParser.Object, mockTranslator.Object, mockHttp.Object);

            // When
            var method = translated ? target.GetTranslated : (Func<string, Task<Pokemon>>)target.Get;
            var res = await method(randomName);

            // Then
            mockParser.Verify(h => h.ToPokemon(expectedJson), Times.Once);
        }

        [Fact]
        public async void WhenGet_ThenAsFremParser()
        {
            // Given
            var mockParser = new Mock<IPokemonParser>();
            var expectedPokemon = new Pokemon();
            mockParser.Setup(h => h.ToPokemon(It.IsAny<string>())).Returns(expectedPokemon);
            var mockHttp = new Mock<IHttpService>();
            var mockTranslator = new Mock<ITranslationService>();
            var randomName = Guid.NewGuid().ToString();
            var target = new PokemonService(mockParser.Object, mockTranslator.Object, mockHttp.Object);

            // When
            var ret = await target.Get(randomName);

            // Then
            Assert.Same(expectedPokemon, ret);
        }

        [Fact]
        public async void WhenTranslated_ThenTranslatedServiceCalled()
        {
            // Given
            var mockParser = new Mock<IPokemonParser>();
            var mockTranslation = new Mock<ITranslationService>();
            var mockHttp = new Mock<IHttpService>();
            var testPokemon = new Pokemon
            {
                Description = Guid.NewGuid().ToString(),
                Habitat = Guid.NewGuid().ToString(),
                IsLegendary = _rnd.Next(2) == 0,
                Name = Guid.NewGuid().ToString()
            };
            mockTranslation.Setup(t => t.Translate(It.IsAny<Pokemon>())).Returns(Task.FromResult(testPokemon));
            var target = new PokemonService(mockParser.Object, mockTranslation.Object, mockHttp.Object);

            // When
            var result = await target.GetTranslated(Guid.NewGuid().ToString());

            // Then
            Assert.NotNull(result);
            Assert.Equal(testPokemon.Name, result.Name);
            Assert.Equal(testPokemon.IsLegendary, result.IsLegendary);
            Assert.Equal(testPokemon.Habitat, result.Habitat);
            Assert.Equal(testPokemon.Description, result.Description);
        }

        #endregion

        #region Experiments

        //[Fact]
        internal async void Find_Legendary()
        {
            var httpService = new HttpService();
            var service = new PokemonService(new PokemonParser(), new TranslationService(httpService, new TranslationParser()), 
                httpService);
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

                    if (p.IsLegendary && p.Habitat == "cave")
                    {
                        // found
                    }

                    if (p.Habitat == "cave" && !p.IsLegendary)
                    {
                        // found
                    }

                    if (p.Habitat != "cave" && p.IsLegendary)
                    {
                        // found
                    }

                    if (p.Habitat != "cave" && !p.IsLegendary)
                    {
                        // found
                    }

                }

            } while (url != null);
        }

        #endregion
    }
}
