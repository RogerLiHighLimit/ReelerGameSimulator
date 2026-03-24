using ReelerGameSimulator.Config;
using ReelerGameSimulator.Logic;
using ReelerGameSimulator.Stats;
using ReelerGameSimulator.Stats.Models;

namespace ReelerGameSimulator
{
    public class GameSimulationTask
    {
        private readonly GameConfig _gameConfig;
        private readonly int _batchId;
        private readonly int _totalBatches;

        public GameSimulationTask(GameConfig gameConfig, int batchId, int totalBatches)
        {
            _gameConfig = gameConfig;
            _batchId = batchId;
            _totalBatches = totalBatches;
        }

        public GameStatsModel Run()
        {
            GameLogic gameLogic = new GameLogic(_gameConfig);
            GameStats gameStats = new GameStats();
            gameLogic.InitialGameState();
            var EventConfig = gameLogic.GameState.EventState; // short cut

            #region initial batches
            List<int> ranges = new List<int>();
            for (int col = 0; col < EventConfig.DisplayConfig.Columns; col++)
            {
                var symbolSetName = EventConfig.DisplayConfig.SymbolSets[col];
                var symbolSet = _gameConfig.SymbolSets[symbolSetName];
                ranges.Add(symbolSet.Symbols.Count);
            }

            int range = ranges[0] / _totalBatches;
            if ((ranges[0] % _totalBatches) != 0)
            {
                // round up to convert full range
                range = ranges[0] / _totalBatches + 1;
            }
            else
            {
                range = ranges[0] / _totalBatches;
            }

            int r0Min = _batchId * range;
            int r0Max = _batchId * range + range;

            if (r0Min > ranges[0])
            {
                r0Min = ranges[0];
            }

            if (r0Max > ranges[0])
            {
                r0Max = ranges[0];
            }

            if (r0Min == r0Max)
            {
                return gameStats.GameStatsModel;
            }

            Console.WriteLine($"{r0Min}:{r0Max}");
            #endregion

            #region batch sim
            List<int> stops = new List<int>() { 0, 0, 0, 0, 0 };
            for (int r0 = r0Min; r0 < r0Max; r0++)
            {
                stops[0] = r0;
                for (int r1 = 0; r1 < ranges[1]; r1++)
                {
                    stops[1] = r1;
                    for (int r2 = 0; r2 < ranges[2]; r2++)
                    {
                        stops[2] = r2;
                        for (int r3 = 0; r3 < ranges[3]; r3++)
                        {
                            stops[3] = r3;
                            for (int r4 = 0; r4 < ranges[4]; r4++)
                            {
                                stops[4] = r4;
                                gameLogic.UpdateDisplay(stops);
                                gameLogic.CheckPayout();
                                gameLogic.DoPayout();

                                gameStats.StatsEvent(gameLogic.GameState);
                            }
                        }
                    }
                }

                if (_batchId == 0)
                {
                    Console.WriteLine($"Task {r0}:{ranges[0]} complete at {DateTimeOffset.UtcNow.LocalDateTime}");
                }
            }
            #endregion

            return gameStats.GameStatsModel;
        }
    }
}
