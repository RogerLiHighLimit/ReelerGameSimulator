namespace ReelerGameSimulator.Config.Data
{
    public class PayTableGroupConfigData
    {
        public string PayoutType { get; set; } = string.Empty;
        public string Type {  get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
        public string Sort { get; set; } = string.Empty;
        public PayTableEntryConfigData[] Entries { get; set; }
    }
}
