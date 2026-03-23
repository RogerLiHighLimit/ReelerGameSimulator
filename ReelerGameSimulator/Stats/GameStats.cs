using ReelerGameSimulator.Logic.Model;
using ReelerGameSimulator.Stats.Models;

namespace ReelerGameSimulator.Stats
{
    public class GameStats
    {
        public GameStatsModel GameStatsModel { get; } = new GameStatsModel();

        public void StatsGamePlay(GameState gameState)
        {
            foreach (var entry in gameState.Payouts)
            {
                if (GameStatsModel.PayoutReasonStats.ContainsKey(entry.Reason))
                {
                    var stats = GameStatsModel.PayoutReasonStats[entry.Reason];
                    stats.Hits++;
                    stats.TotalBet += gameState.EventFinancials.Wager;
                    stats.TotalWin += (long)entry.Amount;
                }
                else
                {
                    GameStatsModel.PayoutReasonStats.Add(entry.Reason, new PayoutTypeStats() { Hits = 1, TotalBet = gameState.EventFinancials.Wager, TotalWin = (long)entry.Amount });
                }
            }
        }
    }
}
