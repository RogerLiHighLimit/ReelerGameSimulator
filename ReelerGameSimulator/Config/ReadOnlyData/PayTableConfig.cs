using ReelerGameSimulator.Config.Data;

namespace ReelerGameSimulator.Config.ReadOnlyData
{
    public class PayTableConfig
    {
        public string Name { get; } = string.Empty;
        public IReadOnlyDictionary<PayTableType, PayTableGroupConfig> Groups { get; }

        public PayTableConfig(PayTableConfigData data)
        {
            Name = data.Name;
            Dictionary<PayTableType, PayTableGroupConfig> _Groups = new Dictionary<PayTableType, PayTableGroupConfig>();
            foreach (var entry in data.Groups)
            {
                _Groups.Add(entry.Type, new PayTableGroupConfig(entry));
            }
            Groups = _Groups;
        }

        public PayTableConfig()
        {
        }
    }
}
