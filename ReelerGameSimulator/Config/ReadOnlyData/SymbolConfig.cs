using ReelerGameSimulator.Config.Models;

namespace ReelerGameSimulator.Config.ReadOnlyData
{
    public class SymbolConfig 
    {
        public int Id { get; } = -1;
        public string Name { get; } = "Unknown";

        public IReadOnlyList<SymbolFlags> Flags { get; } = new List<SymbolFlags>();

        public bool IsWild { get; }
        public bool IsWildSubstitutable { get; }
        public bool IsNoneLineSymbols => !IsWild && !IsWildSubstitutable; 
        public bool IsScatter { get; }

        public SymbolConfig()
        {
        }

        public SymbolConfig(SymbolConfigData data)
        {
            Id = data.Id;
            Name = data.Name;
            Flags = data.Flags;
            IsWild = data.Flags.Contains(SymbolFlags.Wild);
            IsWildSubstitutable = data.Flags.Contains(SymbolFlags.WildSubstitutable);
            IsScatter = data.Flags.Contains(SymbolFlags.Scatter);
        }
    }
}
