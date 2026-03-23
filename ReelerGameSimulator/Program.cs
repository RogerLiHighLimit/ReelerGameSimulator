using ReelerGameSimulator.Config;
using ReelerGameSimulator.DataOutput.ConsoleWriter;
using ReelerGameSimulator.Logic;
using ReelerGameSimulator.Stats;
using SimulatorLib.DataOutput;
using System.Text.Json;

string json = File.ReadAllText("DataInput//GameConfig.json");
var engineConfigurationData = JsonSerializer.Deserialize<GameConfigData>(json);
if (engineConfigurationData != null)
{
    var EngineConfiguration = new GameConfig(engineConfigurationData);
    GameLogic gameLogic = new GameLogic(EngineConfiguration);
    Logger.Configure(LoggingFlags.Console);
    long timeStart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    Console.WriteLine(DateTimeOffset.UtcNow.LocalDateTime);

    GameStats gameStats = new GameStats();

    int totalCycle = 1;
    for (int i = 0; i < totalCycle; i++)
    {
        gameLogic.InitialGameState();
        gameLogic.ProcessEvent();
        gameStats.StatsGamePlay(gameLogic.GameState);

        DataWriter.ShowEventGameState(gameLogic.GameState);
    }

    DataWriter.ShowGamePlayStats(gameStats.GameStatsModel);

    Console.WriteLine(DateTimeOffset.UtcNow.LocalDateTime);
    long timeEnd = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    long timeElapse = timeEnd - timeStart;
    Console.WriteLine(timeElapse);
}

Console.WriteLine("Hello, World!");
