using ReelerGameSimulator.Config.ReadOnlyData;

namespace ReelerGameSimulator.Config
{
    public class GameConfig
    {
        protected GameConfigData Data { get; private set; }

        public IReadOnlyDictionary<string, SymbolConfig> Symbols { get; private set; }
        public IReadOnlyDictionary<string, SymbolSetConfig> SymbolSets { get; private set; }
        public IReadOnlyDictionary<string, DisplayConfig> Displays { get; private set; }

        public IReadOnlyDictionary<string, PayoutProcessorConfig> PayoutProcessors { get; private set; }
        public IReadOnlyDictionary<string, PayTableConfig> PayTables { get; private set; }

        public GameConfig(GameConfigData data)
        {
            Data = data;

            #region Symbols
            Dictionary<string, SymbolConfig> _Symbols = new Dictionary<string, SymbolConfig>();
            foreach (var entry in data.Symbols)
            {
                SymbolConfig config = new SymbolConfig(entry);
                _Symbols.Add(config.Name, config);
            }
            Symbols = _Symbols;
            #endregion

            #region SymbolSets
            Dictionary<string, SymbolSetConfig> _SymbolSets = new Dictionary<string, SymbolSetConfig>();
            foreach (var entry in data.SymbolSets)
            {
                _SymbolSets.Add(entry.Name, new SymbolSetConfig(entry));
            }
            SymbolSets = _SymbolSets;
            #endregion

            #region Displays
            Dictionary<string, DisplayConfig> _Displays = new Dictionary<string, DisplayConfig>();
            foreach (var entry in Data.Displays)
            {
                _Displays.Add(entry.Name, new DisplayConfig(entry));
            }
            Displays = _Displays;
            #endregion

            #region PayoutProcessors
            Dictionary<string, PayoutProcessorConfig> _PayoutProcessors = new Dictionary<string, PayoutProcessorConfig>();
            foreach (var entry in Data.PayoutProcessors)
            {
                _PayoutProcessors.Add(entry.Name, new PayoutProcessorConfig(entry));
            }
            PayoutProcessors = _PayoutProcessors;
            #endregion

            #region
            Dictionary<string, PayTableConfig> _PayTables = new Dictionary<string, PayTableConfig>();
            foreach (var entry in Data.PayTables)
            {
                _PayTables.Add(entry.Name, new PayTableConfig(entry));
            }
            PayTables = _PayTables;
            #endregion
        }
    }
}
