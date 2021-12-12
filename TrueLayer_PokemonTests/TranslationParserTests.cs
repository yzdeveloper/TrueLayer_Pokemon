using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueLayer_Pokemon.Services;
using Xunit;

namespace TrueLayer_PokemonTests
{
    public class TranslationParserTests
    {
        [Fact]
        public void ParserChecksSuccess()
        {
            // Given
            const string source = @"{
  ""success"": {
    ""total"": 1
  },
  ""contents"": {
    ""translated"": ""Thee did giveth mr. Tim a hearty meal,  but unfortunately what he did doth englut did maketh him kicketh the bucket."",
    ""text"": ""You gave Mr. Tim a hearty meal, but unfortunately what he ate made him die."",
    ""translation"": ""shakespeare""
  }
}";
            var target = new TranslationParser();

            // When
            var result = target.Parse(source);

            // Then
            Assert.Equal("Thee did giveth mr. Tim a hearty meal,  but unfortunately what he did doth englut did maketh him kicketh the bucket.", 
                result);
        }
    }
}
