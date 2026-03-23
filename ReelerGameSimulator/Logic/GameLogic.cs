using ReelerGameSimulator.Config;
using ReelerGameSimulator.Logic.Model;
using ReelerGameSimulator.Rng;

namespace ReelerGameSimulator.Logic
{
    public class GameLogic
    {
        public GameConfig GameConfig { get; private set; }
        public GameState GameState { get; private set; } = new GameState();
        public EventState EventConfig => GameState.EventState;
        public RandomNumberGeneratorWapper Rng { get; private set; } = new RandomNumberGeneratorWapper();

        public GameLogic(GameConfig engineConfiguration)
        {
            GameConfig = engineConfiguration;
        }

        public void ProcessEvent()
        {
            UpdateDisplay();
            CheckPayout();
            DoPayout();
        }

        public virtual void InitialGameState()
        {
            EventConfig.DisplayConfig = GameConfig.Displays["BaseRS11"];
            EventConfig.PayoutProcessorConfig = GameConfig.PayoutProcessors["Default"];
            EventConfig.PayTableConfig = GameConfig.PayTables["Default"];

            foreach (var entry in GameConfig.Symbols)
            {
                if (entry.Value.IsWild)
                {
                    EventConfig.WildSymbol = entry.Value;
                    break;
                }
            }
        }

        public virtual void UpdateDisplay()
        {
            if (GameState.EventState.DisplayConfig.Name != GameState.Display.Name)
            {
                GameState.Display = new Display(EventConfig.DisplayConfig);
            }
                
            //List<int> test_Rng_3L1 = new List<int>() { 11, 3, 7, 24, 3 };
            for (int col = 0; col < EventConfig.DisplayConfig.Columns; col++)
            {
                var symbolSetName = EventConfig.DisplayConfig.SymbolSets[col];
                var symbolSet = GameConfig.SymbolSets[symbolSetName];
                int stopIndex = Rng.GetInt32(symbolSet.Symbols.Count);
                //int stopIndex = test_Rng_3L1[col];

                for (int row = 0; row < EventConfig.DisplayConfig.Rows; row++)
                {
                    int stopIndexRounded = (stopIndex + row) % symbolSet.Symbols.Count();
                    var symbolName = symbolSet.Symbols[stopIndexRounded];
                    var symbolConfig = GameConfig.Symbols[symbolName];
                    GameState.Display[col, row].Symbol = symbolConfig; 
                }
            }
        }

        public virtual void CheckPayout()
        {
            GameState.Payouts.Clear();
            var linePays = GameLogicUtility.MatchLineWins(GameState, GameConfig);
            if (linePays.Count > 0)
            {
                GameState.Payouts.AddRange(linePays);
            }
        }

        public void DoPayout()
        {
            long totalPaid = 0;
            foreach (var pay in GameState.Payouts)
            {
                totalPaid += GameLogicUtility.CalculatePayoutAmount(GameState, GameConfig, pay);
            }
            GameState.EventFinancials.TotalWIn = totalPaid;
        }
    }
}
