namespace ReelerGameSimulator.Config.Data
{
    public class PayTableEntryConfigData
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Payout { get; set; }
    }
}
