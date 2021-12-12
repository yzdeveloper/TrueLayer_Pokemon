namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Pokemon parser
    /// </summary>
    public interface IPokemonParser
    {
        /// <summary>
        /// Parse pokemon from json string 
        /// </summary>
        /// <param name="result">json string</param>
        /// <returns>Pokemon object</returns>
        Pokemon ToPokemon(string result);
    }
}