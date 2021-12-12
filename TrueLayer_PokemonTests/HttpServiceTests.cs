using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrueLayer_Pokemon.Services;
using Xunit;

namespace TrueLayer_PokemonTests
{
    public class HttpServiceTests
    {
        [Fact]
        public async void WhenGet_ThenReturned()
        {
            // Given 
            const string url = "https://api.funtranslations.com/translate/yoda.json?text=Master%20Obiwan%20has%20lost%20a%20planet.";
            var target = new HttpService();

            // When 
            var ret = await target.Get(url);

            // Then
            Assert.NotNull(ret);
            Assert.Contains("\"translation\": \"yoda\"", ret);
        }

        [Fact]
        public async void WhenError_ThenThrow()
        {
            // Given 
            const string url = "not url";
            var target = new HttpService();

            // When-Then
            await Assert.ThrowsAsync<WebException>(async () => 
                { 
                    await target.Get(url); 
                });
        }


        [Fact]
        public async void WhenStatusNotFound_ThenNull()
        {
            // Given 
            const string url = "https://pokeapi.co/api/v2/pokemon-species/wolf";
            var target = new HttpService();

            // When 
            var ret = await target.Get(url);

            // Then
            Assert.Null(ret);
        }

    }
}
