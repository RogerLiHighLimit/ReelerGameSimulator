namespace ReelerGameSimulator.Config.Data
{
    public class PayoutProcessorConfigData
    {
        public string Name { get; set; } = string.Empty;
        public string PayTable { get; set; } = string.Empty;
        public List<List<int>> Lines { get; set; } = [];
        public List<int> ScatterIndexes { get; set; } = [];
    }
}
