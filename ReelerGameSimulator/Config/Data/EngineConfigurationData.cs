using ReelerGameSimulator.Config.Models;

namespace ReelerGameSimulator.Config.Data
{
    public class EngineConfigurationData
    {
        public List<SymbolConfigData> Symbols { get; set; } = [];
        public List<SymbolSetConfigData> SymbolSets { get; set; } = [];
        public List<DisplayConfigData> Displays { get; set; } = [];
        public List<PayoutProcessorConfigData> PayoutProcessors { get; set; } = [];
        public List<PayTableConfigData> PayTables { get; set; } = [];
    }
}
