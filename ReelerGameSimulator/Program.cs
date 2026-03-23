using ReelerGameSimulator.Config;
using ReelerGameSimulator.DataOutput.ConsoleWriter;
using ReelerGameSimulator.Logic;
using ReelerGameSimulator.Stats;
using ReelerGameSimulator.Stats.Models;
using SimulatorLib.DataOutput;
using System.Text.Json;

int NumTask = 10;
int TotalCycle = 100_000_00;
int CyclePerTask = TotalCycle / NumTask;
int ReportPercentage = 10;
int CycleReport = CyclePerTask/ ReportPercentage;

Console.WriteLine(DateTimeOffset.UtcNow.LocalDateTime);
long timeStart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

string json = File.ReadAllText("DataInput//GameConfig.json");
var engineConfigurationData = JsonSerializer.Deserialize<GameConfigData>(json);

#region tasks run
var tasks = new List<Task<GameStatsModel>>();
for (int i = 0; i < NumTask; i++)
{
    int copy = i;
    tasks.Add(Task.Run(() =>
    {
        var EngineConfiguration = new GameConfig(engineConfigurationData);
        GameLogic gameLogic = new GameLogic(EngineConfiguration);
        Logger.Configure(LoggingFlags.Console);
        GameStats gameStats = new GameStats();

        for (int j = 0; j < CyclePerTask; j++)
        {
            gameLogic.InitialGameState();
            gameLogic.ProcessEvent();
            gameStats.StatsEvent(gameLogic.GameState, gameStats.GameStatsModel);

            if (copy == 0 && (j % CycleReport) == 0)
            {
                Console.WriteLine($"Task {j*10/CycleReport}% complete at {DateTimeOffset.UtcNow.LocalDateTime}");
            }
        }      
        return gameStats.GameStatsModel;
    }));
}

// Wait for all tasks to complete and collect results
GameStatsModel[] taskResults = await Task.WhenAll(tasks);
#endregion

#region merge the output results
GameStatsModel totalStats = new GameStatsModel();
for (int i = 1; i < taskResults.Length; i++)
{
    var current = taskResults[i];
    totalStats.TotalWagerCycle += current.TotalWagerCycle;
    foreach (var entry in current.PayoutReasonStats)
    {
        if (!totalStats.PayoutReasonStats.ContainsKey(entry.Key))
        {
            totalStats.PayoutReasonStats.Add(entry.Key, entry.Value);
        }
        else
        {
            totalStats.PayoutReasonStats[entry.Key].Hits += entry.Value.Hits;
            totalStats.PayoutReasonStats[entry.Key].TotalWin += entry.Value.TotalWin;
        }
    }
}

DataWriter.ShowGamePlayStats(totalStats);
#endregion

Console.WriteLine(DateTimeOffset.UtcNow.LocalDateTime);
long timeElapse = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timeStart;
Console.WriteLine(timeElapse);
