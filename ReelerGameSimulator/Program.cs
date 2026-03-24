using ReelerGameSimulator;
using ReelerGameSimulator.Config;
using ReelerGameSimulator.Stats.Models;
using SimulatorLib.DataOutput;
using System.Text.Json;

int NumBatch = 12;

Console.WriteLine(DateTimeOffset.UtcNow.LocalDateTime);
long timeStart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

#region game config
string json = File.ReadAllText("DataInput//GameConfig.json");
var gameConfigData = JsonSerializer.Deserialize<GameConfigData>(json);
if (gameConfigData == null)
    return;

var gameConfig = new GameConfig(gameConfigData);
#endregion

#region game logic and stats tasks
var tasks = new List<Task<GameStatsModel>>();

for (int batchId = 0; batchId < NumBatch; batchId++)
{
    int copy = batchId;
    var worker = new GameSimulationTask(gameConfig, copy, NumBatch);
    tasks.Add(Task.Run(() => worker.Run()));
}

// Wait and collect results
GameStatsModel[] taskResults = await Task.WhenAll(tasks);
#endregion

#region merge and output stats results
GameStatsModel totalStats = new GameStatsModel();
for (int i = 0; i < NumBatch; i++)
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

DataWriterUtility.ShowGamePlayStats(totalStats);
#endregion

Console.WriteLine(DateTimeOffset.UtcNow.LocalDateTime);
long timeElapse = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timeStart;
Console.WriteLine(timeElapse);
