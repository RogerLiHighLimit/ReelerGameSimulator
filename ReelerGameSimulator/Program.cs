// See https://aka.ms/new-console-template for more information

using ReelerGameSimulator.Config;
using ReelerGameSimulator.Config.Data;
using ReelerGameSimulator.Logic;
using System.Text.Json;

string json = File.ReadAllText("Data//GameConfig.json");
var engineConfigurationData = JsonSerializer.Deserialize<EngineConfigurationData>(json);
if (engineConfigurationData != null)
{
    var EngineConfiguration = new EngineConfiguration(engineConfigurationData);
    GameLogic gameLogic = new GameLogic(EngineConfiguration);
    gameLogic.Run();
}

Console.WriteLine("Hello, World!");
