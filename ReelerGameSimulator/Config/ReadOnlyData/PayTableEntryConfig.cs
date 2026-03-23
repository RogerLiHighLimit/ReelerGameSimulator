using ReelerGameSimulator.Config.Data;

namespace ReelerGameSimulator.Config.ReadOnlyData
{
    public class PayTableEntryConfig
    {
        public int Id { get; }
        public string Symbol { get; } = string.Empty;
        public int Count { get; }
        public decimal Payout { get; }
        public PayTableEntryConfig(PayTableEntryConfigData data)
        {
            Id = data.Id;
            Symbol = data.Symbol;
            Count = data.Count;
            Payout = data.Payout;
        }
    }
}
