using ReelerGameSimulator.Config.Data;

namespace ReelerGameSimulator.Config.ReadOnlyData
{
    public class PayTableGroupConfig
    {
        public string PayoutType { get; set; } = string.Empty;
        public PayTableType Type { get; set; }
        public string Direction { get; set; } = string.Empty;
        public string Sort { get; set; } = string.Empty;
        public IReadOnlyDictionary<string, PayTableEntryConfig> Entries { get; set; } = new Dictionary<string, PayTableEntryConfig>();

        public PayTableGroupConfig()
        {
        }

        public PayTableGroupConfig(PayTableGroupConfigData data)
        {
            Type = data.Type;
            Direction = data.Direction;
            Sort = data.Sort;

            Dictionary<string, PayTableEntryConfig> _Entries = new Dictionary<string, PayTableEntryConfig>();
            foreach (var entry in data.Entries)
            {
                var count = entry.Count;
                string prizeString = entry.Symbol;
                for (int i = 1; i < entry.Count; i++)
                {
                    prizeString += "," + entry.Symbol;
                }

                _Entries.Add(prizeString, new PayTableEntryConfig(entry));
            }
            Entries = _Entries;
        }
    }
}