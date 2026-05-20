namespace ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration.Data
{
    public class DisplayConfigData 
    {
        public string Name { get; set; } = string.Empty;
        public int Columns { get; set; }
        public int Rows { get; set; }
        public List<string> SymbolSets { get; set; } = new List<string>();
    }
}
