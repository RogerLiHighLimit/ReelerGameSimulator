using ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration.Data;

namespace ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration
{
    public class SymbolSetConfig
    {
        public string Name { get; } = string.Empty;
        public string Type { get; } = string.Empty;

        public IReadOnlyList<string> Symbols { get; } = new List<string>();

        public SymbolSetConfig(SymbolSetConfigData data)
        {
            Name = data.Name;
            Type = data.Type;
            Symbols = data.Symbols;
        }
    }
}
