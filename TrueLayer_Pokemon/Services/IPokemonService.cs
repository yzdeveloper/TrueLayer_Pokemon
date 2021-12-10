using System.Threading.Tasks;

namespace TrueLayer_Pokemon.Services
{
    public interface IPokemonService
    {
        Task<Pokemon> Get(string name);
    }
}