using OfficeOpenXml;
using OfficeOpenXml.Table;
using ReelerGameSimulator.Stats.Models;

namespace SimulatorLib.DataOutput
{
    public class DataWriterUtility
    {
        public static void ShowGamePlayStats(GameStatsModel gameStatsModel)
        {
            List<string> rowHeaders = new List<string>() {
                "5xWD", "4xWD", "3xWD",
                "5xH1", "4xH1", "3xH1",
                "5xH2", "4xH2", "3xH2",
                "5xH3", "4xH3", "3xH3",
                "5xH4", "4xH4", "3xH4",
                "5xL1", "4xL1", "3xL1",
                "5xL2", "4xL2", "3xL2",
                "5xL3", "4xL3", "3xL3",
                "5xL4", "4xL4", "3xL4",
                "5xL5", "4xL5", "3xL5",
            };

            List<PayItemStats> list = new List<PayItemStats>();

            var sortedDict = gameStatsModel.PayoutReasonStats.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            foreach (var key in rowHeaders)
            {
                var current = gameStatsModel.PayoutReasonStats[key];
                decimal rtp = (decimal)current.TotalWin / (10 * gameStatsModel.TotalWagerCycle);
                var currentItem = new PayItemStats() { Name = key, Hits = current.Hits, Rtp = rtp };
                list.Add(currentItem);
            }

            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("RtpPaylines");

                // Load list into worksheet
                ws.Cells["A1"].LoadFromCollection(list, true);

                // Format RTP column as %
                ws.Column(3).Style.Numberformat.Format = "0.000000%";

                // Auto fit columns
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                // Create table (include all 3 columns)
                var range = ws.Cells[1, 1, list.Count + 1, 3];
                var table = ws.Tables.Add(range, "RtpPaylines");

                table.TableStyle = TableStyles.None;
                table.ShowFilter = false;

                package.SaveAs(new FileInfo("RtpPaylines.xlsx"));
            }
        }
    }

    public class PayItemStats
    {
        public string? Name { get; set; }
        public long Hits { get; set; }
        public decimal Rtp { get; set; }
    }
}
