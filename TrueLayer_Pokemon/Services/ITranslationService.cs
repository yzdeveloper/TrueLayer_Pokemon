using System.Threading.Tasks;

namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Translate pokemon
    /// </summary>
    public interface ITranslationService
    {
        /// <summary>
        /// Translate pokemon description if needed
        /// </summary>
        /// <param name="pokemon">The pokemon object untranslated</param>
        /// <returns>The pokemon object translated</returns>
        Task<Pokemon> Translate(Pokemon pokemon);
    }
}