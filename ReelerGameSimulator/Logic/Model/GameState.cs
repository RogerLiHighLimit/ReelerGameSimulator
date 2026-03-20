using ReelerGameSimulator.Config;

namespace ReelerGameSimulator.Logic.Model
{
    public class GameState
    {
        public Display Display { get; set; } = new Display();
        public List<PayoutResult> Payouts { get; set; } = [];

        public EventConfig EventConfig { get; set; } = new EventConfig();
        public object? EventScratch { get; set; }
    }
}
