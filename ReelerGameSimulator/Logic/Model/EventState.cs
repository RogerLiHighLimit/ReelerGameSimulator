using ReelerGameSimulator.Config.Data;
using ReelerGameSimulator.Config.ReadOnlyData;

namespace ReelerGameSimulator.Logic.Model
{
    public class EventState
    {
        public DisplayConfig DisplayConfig { get; internal set; } = new DisplayConfig();
        public PayoutProcessorConfig PayoutProcessorConfig { get; internal set; } = new PayoutProcessorConfig();
        public PayTableConfig PayTableConfig { get; internal set; } = new PayTableConfig();
        public SymbolConfig WildSymbol { get; internal set; } = new SymbolConfig(); // roger pending

        public PayTableGroupConfig PaytableLines => PayTableConfig.Groups[PayTableType.Payline];
        public PayTableGroupConfig PaytableScatter => PayTableConfig.Groups[PayTableType.Scatter];
    }
}
