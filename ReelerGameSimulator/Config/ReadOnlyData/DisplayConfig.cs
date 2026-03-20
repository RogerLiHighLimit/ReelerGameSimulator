using ReelerGameSimulator.Config.Data;

namespace ReelerGameSimulator.Config.ReadOnlyData
{
    public class DisplayConfig 
    {
        public string Name { get; } = string.Empty;
        public int Columns { get; } = 5;
        public int Rows { get; } = 3;
        public IReadOnlyList<string> SymbolSets { get; } = new List<string>();

        public DisplayConfig()
        {

        }

        public DisplayConfig(DisplayConfigData data)
        {
            Name = data.Name;
            Columns = data.Columns;
            Rows = data.Rows;
            SymbolSets = data.SymbolSets;
        }
    }
}
