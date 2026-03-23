using ReelerGameSimulator.DataOutput.ConsoleWriter;
using ReelerGameSimulator.Logic.Model;
using ReelerGameSimulator.Stats.Models;

namespace SimulatorLib.DataOutput
{
    public class DataWriter
    {
        public static void ShowEventGameState(GameState st)
        {
            var grid = new Table(new string[] { "R0", "R1", "R2", "R3", "R4" }, Alignment.Left)
            {
                ItemAlignment = Alignment.Left,
                Indentation = 5
            };

            var test = st.Display.Symbols.Select(x => x.Symbol.Name).ToList();

            grid.AddRow([test[0], test[1], test[2], test[3], test[4]]);
            grid.AddRow([test[5], test[6], test[7], test[8], test[9]]);
            grid.AddRow([test[10], test[11], test[12], test[13], test[14]]);

            Logger.Write(grid);
        }

        public static void ShowGamePlayStats(GameStatsModel gameStatsModel)
        {
            var sortedDict = gameStatsModel.PayoutReasonStats.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            foreach (var entry in sortedDict)
            {
                decimal rtp = (decimal)entry.Value.TotalWin/(10* gameStatsModel.TotalWagerCycle);
                Console.WriteLine($"{entry.Key}, Hits={entry.Value.Hits}, Rtp={rtp}");
            }
        }
    }
}
