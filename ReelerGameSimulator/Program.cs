using ReelerGameSimulator.Config;
using ReelerGameSimulator.Logic;
using ReelerGameSimulator.View;
using System.Text.Json;

string json = File.ReadAllText("Data//GameConfig.json");
var engineConfigurationData = JsonSerializer.Deserialize<GameConfigData>(json);
if (engineConfigurationData != null)
{
    var EngineConfiguration = new GameConfig(engineConfigurationData);
    GameLogic gameLogic = new GameLogic(EngineConfiguration);
    Logger.Configure(LoggingFlags.Console);
    long timeStart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    gameLogic.InitialEventConfig();
    gameLogic.ProcessEvent();
    gameLogic.ShowProcessEventResult();

    long timeEnd = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    long timeElapse = timeEnd - timeStart;
    Console.WriteLine(timeElapse);
}

Console.WriteLine("Hello, World!");
