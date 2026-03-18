using ReelerGameSimulator.Config.Data;

namespace ReelerGameSimulator.Config
{
    public class EngineConfiguration
    {
        public EngineConfigurationData Data { get; private set; }

        public EngineConfiguration(EngineConfigurationData data)
        {
            Data = data;
        }
    }
}
