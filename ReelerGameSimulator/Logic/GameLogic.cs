using ReelerGameSimulator.Config;
using ReelerGameSimulator.Logic.Model;
using ReelerGameSimulator.Rng;
using ReelerGameSimulator.View;

namespace ReelerGameSimulator.Logic
{
    public class GameLogic
    {
        public GameConfig EngineConfiguration { get; private set; }
        public GameState GameState { get; private set; } = new GameState();
        public EventConfig EventConfig => GameState.EventConfig;
        public RandomNumberGeneratorWapper Rng { get; private set; } = new RandomNumberGeneratorWapper();

        public GameLogic(GameConfig engineConfiguration)
        {
            EngineConfiguration = engineConfiguration;
        }

        public void ProcessEvent()
        {
            UpdateDisplay();
            CheckPayout();
            DoPayout();
        }

        public virtual void InitialEventConfig()
        {
            EventConfig.DisplayConfig = EngineConfiguration.Displays["BaseRS11"];
            EventConfig.PayoutProcessorConfig = EngineConfiguration.PayoutProcessors["Default"];
            EventConfig.PayTableConfig = EngineConfiguration.PayTables["Default"];

            foreach (var entry in EngineConfiguration.Symbols)
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
            if (GameState.EventConfig.DisplayConfig.Name != GameState.Display.Name)
            {
                GameState.Display = new Display(EventConfig.DisplayConfig);
            }
                
            List<int> test_Rng_3L1 = new List<int>() { 11, 3, 7, 24, 3 };
            for (int col = 0; col < EventConfig.DisplayConfig.Columns; col++)
            {
                var symbolSetName = EventConfig.DisplayConfig.SymbolSets[col];
                var symbolSet = EngineConfiguration.SymbolSets[symbolSetName];
                //int stopIndex = Rng.GetInt32(symbolSet.Symbols.Count);
                int stopIndex = test_Rng_3L1[col];

                for (int row = 0; row < EventConfig.DisplayConfig.Rows; row++)
                {
                    int stopIndexRounded = (stopIndex + row) % symbolSet.Symbols.Count();
                    var symbolName = symbolSet.Symbols[stopIndexRounded];
                    var symbolConfig = EngineConfiguration.Symbols[symbolName];
                    GameState.Display[col, row].Symbol = symbolConfig; 
                }
            }
        }

        public virtual void CheckPayout()
        {
            GameState.Payouts.Clear();
            var linePays = GameLogicUtility.MatchLineWins(GameState, EngineConfiguration);
            if (linePays.Count > 0)
            {
                GameState.Payouts.AddRange(linePays);
            }
        }

        public void DoPayout()
        {
            decimal totalPaid = 0;
            for (int i = 0; i < GameState.Payouts.Count; i++)
            {

            }
        }

        public virtual void ShowProcessEventResult()
        {
            var grid = new Table(new string[] { "R0", "R1", "R2", "R3", "R4" }, Alignment.Left)
            {
                ItemAlignment = Alignment.Left,
                Indentation = 5
            };

            var test = GameState.Display.Symbols.Select(x => x.Symbol.Name).ToList();

            //grid.AddRow(new string[] { test[0], test[1], test[2], test[3], test[4] });

            grid.AddRow([test[0], test[1], test[2], test[3], test[4]]);
            grid.AddRow([test[5], test[6], test[7], test[8], test[9]]);
            grid.AddRow([test[10], test[11], test[12], test[13], test[14]]);

            Logger.Write(grid);
        }
    }
}
