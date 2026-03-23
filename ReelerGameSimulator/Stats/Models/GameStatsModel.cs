
using ReelerGameSimulator.Logic.Model;

namespace ReelerGameSimulator.Stats.Models
{
    public class GameStatsModel
    {
        public Dictionary<string, PayoutTypeStats> PayoutReasonStats { get; } = new Dictionary<string, PayoutTypeStats>();
    }
}
