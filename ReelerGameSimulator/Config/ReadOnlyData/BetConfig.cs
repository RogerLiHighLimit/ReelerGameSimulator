using ReelerGameSimulator.Config.Data;

namespace ReelerGameSimulator.Config.ReadOnlyData
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
