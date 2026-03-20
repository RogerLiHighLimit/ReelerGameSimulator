using ReelerGameSimulator.Config.Data;

namespace ReelerGameSimulator.Config.ReadOnlyData
{
    public class PayTableEntryConfig
    {
        public int Id { get; }
        public decimal Payout { get; }
        public PayTableEntryConfig(PayTableEntryConfigData data)
        {
            Id = data.Id;
            Payout = data.Payout;
        }
    }
}
