using System.Threading.Tasks;

namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Pokemon service 
    /// </summary>
    public interface IPokemonService
    {
        /// <summary>
        /// Get pokemon object from name
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>pokemon object</returns>
        Task<Pokemon> Get(string name);


        /// <summary>
        /// Get pokemon object from name, Description of the pokemon will be translated
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>pokemon object</returns>
        Task<Pokemon> GetTranslated(string name);
    }
}