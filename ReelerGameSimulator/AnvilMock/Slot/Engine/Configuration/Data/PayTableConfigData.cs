namespace ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration.Data
{
    public class PayTableConfigData 
    {
        public string Name { get; set; } = string.Empty;
        public List<PayTableGroupConfigData> Groups { get; set; } = new List<PayTableGroupConfigData>();
    }
}
