namespace ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration.Data
{
    public class EngineConfigurationData 
    {
        public BetConfigData Bet { get; set; } = new BetConfigData();
        public List<SymbolConfigData> Symbols { get; set; } = [];
        public List<SymbolSetConfigData> SymbolSets { get; set; } = [];
        public List<DisplayConfigData> Displays { get; set; } = [];
        public List<PayoutProcessorConfigData> PayoutProcessors { get; set; } = [];
        public List<PayTableConfigData> PayTables { get; set; } = [];
    }
}
