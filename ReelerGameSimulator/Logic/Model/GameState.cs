using ReelerGameSimulator.Config;

namespace ReelerGameSimulator.Logic.Model
{
    public class GameState
    {
        public Financials EventFinancials { get; set; } = new Financials();
        public EventState EventState { get; set; } = new EventState();

        public Display Display { get; set; } = new Display();
        public List<PayoutResult> Payouts { get; set; } = [];

        public object? EventScratch { get; set; }
    }
}
