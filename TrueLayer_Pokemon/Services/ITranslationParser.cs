namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Parser for the translation API
    /// </summary>
    public interface ITranslationParser
    {
        /// <summary>
        /// Parse the transaltion service response into translation result
        /// </summary>
        /// <param name="json">the translation service response</param>
        /// <returns>translation</returns>
        string Parse(string json);
    }


}