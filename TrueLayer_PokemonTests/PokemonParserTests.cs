using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrueLayer_Pokemon.Services;
using Xunit;

namespace TrueLayer_PokemonTests
{
    public class PokemonParserTests
    {
        public static IEnumerable<object[]> GetParsingTest()
        {
            // name description habitat not null, false legendary
            yield return new object[] { 
                GetResource("bulbasaur.txt"),
                "A strange seed was\nplanted on its\nback at birth.\fThe plant sprouts\nand grows with\nthis POKéMON.",
                "grassland", false,
                "bulbasaur" };
            // habitat null, false legendary
            yield return new object[] {
                GetResource("wormadam.txt"),
                "When BURMY evolved, its cloak\nbecame a part of this Pokémon’s\nbody. The cloak is never shed.",
                null, false,
                "wormadam" };
            // true legendary
            yield return new object[] {
                GetResource("articuno.txt"),
                "A legendary bird\nPOKéMON that is\nsaid to appear todoomed people who\nare lost in icy\nmountains.",
                "rare", true,
                "articuno" };
        }

        private static object GetResource(string name)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TrueLayer_PokemonTests.{name}");
            using var tr = new StreamReader(stream);
            return tr.ReadToEnd();
        }

        [Theory]
        [MemberData(nameof(GetParsingTest))]
        public void Parser_Working(string serviceResult, string description, string habitat, bool isLegendary, string name)
        {
            // Given
            var target = new PokemonParser();

            // When
            var result = target.ToPokemon(serviceResult);

            // Then
            Assert.NotNull(result);
            Assert.Equal(description, result.Description);
            Assert.Equal(habitat, result.Habitat);
            Assert.Equal(isLegendary, result.IsLegendary);
            Assert.Equal(name, result.Name);
        }
    }
}
