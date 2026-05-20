using ReelerGameSimulator.Config.Data;
using ReelerGameSimulator.Config.ReadOnlyData;
using System.Linq;

namespace ReelerGameSimulator.Config
{
    public class GameConfig
    {
        protected GameConfigData Data { get; private set; }

        public BetConfig BetConfig { get; private set; }
        public IReadOnlyDictionary<string, SymbolConfig> Symbols { get; private set; }
        public IReadOnlyDictionary<string, SymbolSetConfig> SymbolSets { get; private set; }
        public IReadOnlyDictionary<string, DisplayConfig> Displays { get; private set; }

        public IReadOnlyDictionary<string, PayoutProcessorConfig> PayoutProcessors { get; private set; }
        public IReadOnlyDictionary<string, PayTableConfig> PayTables { get; private set; }

        public GameConfig(GameConfigData data)
        {
            Data = data;

            BetConfig = new BetConfig(data.Bet);

            #region Symbols
            IReadOnlyList<SymbolConfig> symbolConfigList = data.Symbols.Select(p => new SymbolConfig(p)).ToList().AsReadOnly();
            Symbols = symbolConfigList.ToDictionary(o => o.Name);
            #endregion

            #region SymbolSets
            IReadOnlyList<SymbolSetConfig> symbolSetsList = data.SymbolSets.Select(p => new SymbolSetConfig(p)).ToList().AsReadOnly();
            SymbolSets = symbolSetsList.ToDictionary(o => o.Name);
            #endregion

            #region Displays
            IReadOnlyList<DisplayConfig> displayList = data.Displays.Select(p => new DisplayConfig(p)).ToList().AsReadOnly();
            Displays = displayList.ToDictionary(o => o.Name);
            #endregion

            #region PayoutProcessors
            IReadOnlyList<PayoutProcessorConfig> payoutProcessorList = data.PayoutProcessors.Select(p => new PayoutProcessorConfig(p)).ToList().AsReadOnly();
            PayoutProcessors = payoutProcessorList.ToDictionary(o => o.Name);
            #endregion

            #region PayTables
            IReadOnlyList<PayTableConfig> paytableList = data.PayTables.Select(p => new PayTableConfig(p)).ToList().AsReadOnly();
            PayTables = paytableList.ToDictionary(o => o.Name);
            #endregion
        }
    }
}
