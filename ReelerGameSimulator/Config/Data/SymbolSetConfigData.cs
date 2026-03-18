namespace ReelerGameSimulator.Config.Models
{
    public class SymbolSetConfigData
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public List<string> Symbols { get; set; } = new List<string>();
    }
}
