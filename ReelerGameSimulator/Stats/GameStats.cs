using ReelerGameSimulator.Logic.Model;
using ReelerGameSimulator.Stats.Models;

namespace ReelerGameSimulator.Stats
{
    public class GameStats
    {
        public GameStatsModel GameStatsModel { get; } = new GameStatsModel();

        public void StatsEvent(GameState gameState, GameStatsModel gameStatsModel)
        {
            gameStatsModel.TotalWagerCycle++;

            foreach (var entry in gameState.Payouts)
            {
                if (GameStatsModel.PayoutReasonStats.ContainsKey(entry.Reason))
                {
                    var stats = GameStatsModel.PayoutReasonStats[entry.Reason];
                    stats.Hits++;
                    stats.TotalWin += (long)entry.Amount;
                }
                else
                {
                    GameStatsModel.PayoutReasonStats.Add(entry.Reason, new PayoutTypeStats() { Hits = 1, TotalWin = (long)entry.Amount });
                }
            }
        }
    }
}
