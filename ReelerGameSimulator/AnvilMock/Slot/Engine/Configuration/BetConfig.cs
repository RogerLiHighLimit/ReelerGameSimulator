using ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration.Data;

namespace ReelerGameSimulator.AnvilMock.Slot.Engine.Configuration
{
    public class BetConfig
    {
        public long Multiplier { get; }

        public BetConfig(BetConfigData data)
        {
            Multiplier = data.Multiplier;
        }
    }
}
