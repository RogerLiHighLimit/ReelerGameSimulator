namespace ReelerGameSimulator.Config.Models
{
    public class SymbolFlags
    {

        public const string None = "None";
        //
        // Summary:
        //     The symbol will accept any wild symbol as a substitue during payout analysis.
        public const string WildSubstitutable = "WildSubstitutable";
        //
        // Summary:
        //     The symbol is considered to be wild during payout analysis.
        public const string Wild = "Wild";
        //
        // Summary:
        //     The symbol is considered to be a scatter.
        public const string Scatter = "Scatter";
        //
        // Summary:
        //     The symbol is considered to be both a wild and scatter.
        public const string WildScatter = "WildScatter";
    }

    public class SymbolConfigData
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = string.Empty;

        public HashSet<string> Flags { get; set; } = new HashSet<string>();
    }
}
