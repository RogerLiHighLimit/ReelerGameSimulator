using ReelerGameSimulator.Config;
using ReelerGameSimulator.Logic;
using ReelerGameSimulator.Stats;
using ReelerGameSimulator.Stats.Models;

namespace ReelerGameSimulator
{
    public class GameSimulationTask
    {
        private const int Batches = 10;

        private readonly GameConfig _gameConfig;
        private readonly int _taskId;
        private readonly int _batchCycles;

        public GameSimulationTask(GameConfig gameConfig, int taskId, int cyclePerTask)
        {
            _gameConfig = gameConfig;
            _taskId = taskId;
            _batchCycles = cyclePerTask/Batches;
        }

        public GameStatsModel Run()
        {
            GameLogic gameLogic = new GameLogic(_gameConfig);
            GameStats gameStats = new GameStats();

            for (int i = 1; i <= Batches; i++)
            {
                for (int j = 1; j <= _batchCycles; j++)
                {
                    gameLogic.InitialGameState();
                    gameLogic.ProcessEvent();

                    gameStats.StatsEvent(gameLogic.GameState);
                }

                if (_taskId == 0)
                {
                    // batch report
                    Console.WriteLine($"Task {i * Batches}% complete at {DateTimeOffset.UtcNow.LocalDateTime}");
                }
            }

            return gameStats.GameStatsModel;
        }
    }
}
