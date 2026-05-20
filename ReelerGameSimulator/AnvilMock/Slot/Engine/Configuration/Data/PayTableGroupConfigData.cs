using System.Text.Json.Serialization;

namespace ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration.Data
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PayTableType
    {
        Payline,
        Scatter,
        Ways,
        Cluster,
    }

    public class PayTableGroupConfigData 
    {
        public string PayoutType { get; set; } = string.Empty;
        public PayTableType Type { get; set; }
        public string Direction { get; set; } = string.Empty;
        public string Sort { get; set; } = string.Empty;
        public PayTableEntryConfigData[] Entries { get; set; }
    }
}
