namespace TrueLayer_Pokemon.Services
{
    public interface IPokemonParser
    {
        Pokemon ToPokemon(string result);
    }
}