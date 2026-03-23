using OfficeOpenXml;
using OfficeOpenXml.Table;
using ReelerGameSimulator.Stats.Models;

namespace SimulatorLib.DataOutput
{
    public class DataWriter
    {
        public static void ShowGamePlayStats(GameStatsModel gameStatsModel)
        {
            List<PayItemStats> list = new List<PayItemStats>();

            var sortedDict = gameStatsModel.PayoutReasonStats.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            foreach (var entry in sortedDict)
            {
                decimal rtp = (decimal)entry.Value.TotalWin/(10* gameStatsModel.TotalWagerCycle);
                var currentItem = new PayItemStats() { Name = entry.Key, Hits = entry.Value.Hits, Rtp = rtp };
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
