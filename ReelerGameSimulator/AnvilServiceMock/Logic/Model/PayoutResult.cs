using ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration;
using ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration.Data;

namespace ReelerGameSimulator.AnvilServiceMock.Logic.Model
{
    public class PayoutResult
    {
        public int Id { get; set; }
        public PayTableType Type { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
        public int Multiplier { get; set; }
        public int Payline { get; set; }
        public string PaylineDefinition { get; set; }
        public List<int> Indexes { get; set; }
        public int Wilds { get; set; }

        public PayoutResult(PayTableEntryConfig payTableEntryConfig, PayTableType type, int multiplier, int lineId, string paylineDefinition, List<int> indexes, int numWild)
        {
            Id = payTableEntryConfig.Id;
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
