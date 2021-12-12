using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueLayer_Pokemon.Services;
using Xunit;

namespace TrueLayer_PokemonTests
{
    public class TranslationServiceTests
    {
        private static readonly Random _rnd = new();

        [Fact]
        public async void WhenTranslate_ThenDescriptionAsPerParser()
        {
            // Given
            var mockHttp = new Mock<IHttpService>();
            var mockParser = new Mock<ITranslationParser>();
            var tarnslated = Guid.NewGuid().ToString();
            mockParser.Setup(p => p.Parse(It.IsAny<string>())).Returns(tarnslated);
            var target = new TranslationService(mockHttp.Object, mockParser.Object);
            var subject = new Pokemon
            {
                Description = Guid.NewGuid().ToString(),
                Habitat = Guid.NewGuid().ToString(),
                IsLegendary = _rnd.Next(2) == 0,
                Name = Guid.NewGuid().ToString()
            };

            // When
            var result = await target.Translate(subject);

            // Then
            Assert.NotNull(result);
            Assert.Equal(tarnslated, result.Description);
            Assert.Equal(subject.Habitat, result.Habitat);
            Assert.Equal(subject.IsLegendary, result.IsLegendary);
            Assert.Equal(subject.Name, result.Name);
        }

        [Fact]
        public async void WhenTranslateAndParserThrow_ThenDescriptionAsPerParser()
        {
            // Given
            var mockHttp = new Mock<IHttpService>();
            var mockParser = new Mock<ITranslationParser>();
            var tarnslated = Guid.NewGuid().ToString();
            mockParser.Setup(p => p.Parse(It.IsAny<string>())).Throws<InvalidOperationException>();
            var target = new TranslationService(mockHttp.Object, mockParser.Object);
            var subject = new Pokemon
            {
                Description = Guid.NewGuid().ToString(),
                Habitat = Guid.NewGuid().ToString(),
                IsLegendary = _rnd.Next(2) == 0,
                Name = Guid.NewGuid().ToString()
            };

            // When
            var result = await target.Translate(subject);

            // Then
            Assert.NotNull(result);
            Assert.Equal(subject.Description, result.Description);
            Assert.Equal(subject.Habitat, result.Habitat);
            Assert.Equal(subject.IsLegendary, result.IsLegendary);
            Assert.Equal(subject.Name, result.Name);
        }

        [Fact]
        public async void WhenTranslateAndGetThrow_ThenDescriptionAsPerParser()
        {
            // Given
            var mockHttp = new Mock<IHttpService>();
            var mockParser = new Mock<ITranslationParser>();
            var tarnslated = Guid.NewGuid().ToString();
            mockHttp.Setup(p => p.Get(It.IsAny<string>())).Throws<InvalidOperationException>();
            var target = new TranslationService(mockHttp.Object, mockParser.Object);
            var subject = new Pokemon
            {
                Description = Guid.NewGuid().ToString(),
                Habitat = Guid.NewGuid().ToString(),
                IsLegendary = _rnd.Next(2) == 0,
                Name = Guid.NewGuid().ToString()
            };

            // When
            var result = await target.Translate(subject);

            // Then
            Assert.NotNull(result);
            Assert.Equal(subject.Description, result.Description);
            Assert.Equal(subject.Habitat, result.Habitat);
            Assert.Equal(subject.IsLegendary, result.IsLegendary);
            Assert.Equal(subject.Name, result.Name);
        }

        [Theory]
        [InlineData("cave", true)]
        [InlineData("cave", false)]
        [InlineData("grassland", false)]
        [InlineData("grassland", true)]
        public async void WhenTranslate_ThenParserAsPerHttp(string habitat, bool legendary)
        {
            // Given
            var mockHttp = new Mock<IHttpService>();
            var translated = Guid.NewGuid().ToString();
            mockHttp.Setup(h => h.Get(It.IsAny<string>())).Returns(Task.FromResult(translated));
            var mockParser = new Mock<ITranslationParser>();
            var target = new TranslationService(mockHttp.Object, mockParser.Object);
            var subject = new Pokemon
            {
                Description = Guid.NewGuid().ToString(),
                Habitat = habitat,
                IsLegendary = legendary,
                Name = Guid.NewGuid().ToString()
            };

            // When
            var result = await target.Translate(subject);

            // Then
            mockParser.Verify(p => p.Parse(translated), Times.Once);
            mockParser.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData("cave", true, "https://api.funtranslations.com/translate/yoda.json?text={0}")]
        [InlineData("cave", false, "https://api.funtranslations.com/translate/yoda.json?text={0}")]
        [InlineData("grassland", false, "https://api.funtranslations.com/translate/shakespeare.json?text={0}")]
        [InlineData("grassland", true, "https://api.funtranslations.com/translate/yoda.json?text={0}")]
        public async void WhenTranslate_ThenUrlAsExpected(string habitat, bool legendary, string expectedUrlPattern)
        {
            // Given
            var mockHttp = new Mock<IHttpService>();
            var mockParser = new Mock<ITranslationParser>();
            var target = new TranslationService(mockHttp.Object, mockParser.Object);
            var subject = new Pokemon
            {
                Description = Guid.NewGuid().ToString(),
                Habitat = habitat,
                IsLegendary = legendary,
                Name = Guid.NewGuid().ToString()
            };

            // When
            var result = await target.Translate(subject);

            // Then
            var expectedUrl = string.Format(expectedUrlPattern, subject.Description);
            mockHttp.Verify(p => p.Get(expectedUrl), Times.Once);
            mockHttp.VerifyNoOtherCalls();
        }
    }
}
