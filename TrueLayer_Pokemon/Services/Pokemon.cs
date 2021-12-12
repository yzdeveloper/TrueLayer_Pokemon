namespace TrueLayer_Pokemon.Services
{
    /// <summary>
    /// Pokemon DTO
    /// </summary>
    public class Pokemon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
    }
}