
using ReelerGameSimulator.Logic.Model;

namespace ReelerGameSimulator.Stats.Models
{
    public class GameStatsModel
    {
        public long TotalWagerCycle { get; set; } = 0;
        public Dictionary<string, PayoutTypeStats> PayoutReasonStats { get; set; } = new Dictionary<string, PayoutTypeStats>();
    }
}
