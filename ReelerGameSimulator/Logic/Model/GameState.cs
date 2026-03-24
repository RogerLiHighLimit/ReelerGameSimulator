namespace ReelerGameSimulator.Logic.Model
{
    public class GameState
    {
        public Display Display { get; set; } = new Display();
        public List<PayoutResult> Payouts { get; set; } = [];
        
        public Financials EventFinancials { get; set; } = new Financials();
        public EventState EventState { get; set; } = new EventState();
        public object? EventScratch { get; set; }
    }
}
