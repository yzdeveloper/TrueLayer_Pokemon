using System.Net;
using System.Threading.Tasks;

namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Http request wrapper
    /// </summary>
    public class HttpService : IHttpService
    {
        /// <summary>
        /// Get string from Http url
        /// </summary>
        /// <param name="url">the http url</param>
        /// <returns>content of the body http response as string, or null if the http status NOT FOUND</returns>
        /// <exception cref="WebException or else" />
        public async Task<string> Get(string url)
        {
            using var webClient = new WebClient();
            try
            {
                return await webClient.DownloadStringTaskAsync(url); ;
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse wr && wr.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }
    }
}
