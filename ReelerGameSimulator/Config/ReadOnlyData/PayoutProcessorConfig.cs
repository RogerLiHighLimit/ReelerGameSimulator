using ReelerGameSimulator.Config.Data;

namespace ReelerGameSimulator.Config.ReadOnlyData
{
    public class PayoutProcessorConfig 
    {
        public string Name { get; } = string.Empty;
        public string PayTable { get; } = string.Empty;
        public IReadOnlyList<List<int>> Lines { get; } = [];
        public IReadOnlyList<int> ScatterIndexes { get; } = [];

        public PayoutProcessorConfig()
        {

        }

        public PayoutProcessorConfig(PayoutProcessorConfigData data)
        {
            Name = data.Name;
            PayTable = data.PayTable;
            Lines = data.Lines;
            ScatterIndexes = data.ScatterIndexes;
        }
    }
}