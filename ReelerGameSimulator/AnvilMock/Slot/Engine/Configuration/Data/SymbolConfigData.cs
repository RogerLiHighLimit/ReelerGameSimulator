using System.Text.Json.Serialization;

namespace ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration.Data
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SymbolFlags
    {
        None = 0,
        WildSubstitutable = 1,
        Wild = 2,
        Scatter = 4,
        WildScatter = Scatter | Wild, // 0x00000006
    }

    public class SymbolConfigData 
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = string.Empty;

        public List<SymbolFlags> Flags { get; set; } = [];
    }
}
