using System.Threading.Tasks;

namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Http call abstraction
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Get string from Http url
        /// </summary>
        /// <param name="url">the http url</param>
        /// <returns>content of the body http response as string, or null if the http status NOT FOUND</returns>
        Task<string> Get(string url); 
    }
}
