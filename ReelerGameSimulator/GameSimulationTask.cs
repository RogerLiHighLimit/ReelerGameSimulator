using ReelerGameSimulator.Config;
using ReelerGameSimulator.Logic;
using ReelerGameSimulator.Stats;
using ReelerGameSimulator.Stats.Models;

namespace ReelerGameSimulator
{
    public class GameSimulationTask
    {
        private readonly GameConfig _gameConfig;
        private readonly int _taskId;
        private readonly int _cyclePerTask;
        private readonly int _cycleReport;

        public GameSimulationTask(GameConfig gameConfig, int taskId, int cyclePerTask, int cycleReport)
        {
            _gameConfig = gameConfig;
            _taskId = taskId;
            _cyclePerTask = cyclePerTask;
            _cycleReport = cycleReport;
        }

        public GameStatsModel Run()
        {
            GameLogic gameLogic = new GameLogic(_gameConfig);
            GameStats gameStats = new GameStats();

            for (int j = 0; j < _cyclePerTask; j++)
            {
                gameLogic.InitialGameState();
                gameLogic.ProcessEvent();

                if (_taskId == 0 && (j % _cycleReport) == 0 && j != 0)
                {
                    Console.WriteLine($"Task {j * 100 / _cyclePerTask}% complete at {DateTimeOffset.UtcNow.LocalDateTime}");
                }

                gameStats.StatsEvent(gameLogic.GameState);
            }

            return gameStats.GameStatsModel;
        }
    }
}
