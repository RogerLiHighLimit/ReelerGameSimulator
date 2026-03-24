using ReelerGameSimulator.Config.Data;
using ReelerGameSimulator.Config.ReadOnlyData;

namespace ReelerGameSimulator.Logic.Model
{
    public class PayoutResult
    {
        public int Id { get; private set; }
        public PayTableType Type { get; private set; }
        public string Reason { get; private set; }
        public decimal Amount { get; private set; }
        public int Multiplier { get; private set; }
        public int Payline { get; private set; }
        public string PaylineDefinition { get; private set; }
        public List<int> Indexes { get; private set; }
        public int Wilds { get; private set; }

        public PayoutResult(PayTableEntryConfig payTableEntryConfig, PayTableType type, int multiplier, int lineId, string paylineDefinition, List<int> indexes, int numWild)
        {
            Id = payTableEntryConfig.Id;
            Type = type;
            Reason = payTableEntryConfig.Count + "x" + payTableEntryConfig.Symbol;
            Amount = payTableEntryConfig.Payout;
            Multiplier = multiplier;
            Payline = lineId;
            PaylineDefinition = paylineDefinition;
            Indexes = indexes;
            Wilds = numWild;
        }
    }
}
