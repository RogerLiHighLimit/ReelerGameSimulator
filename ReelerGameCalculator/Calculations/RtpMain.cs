using Calculator2Henry;
using Calculator2Henry.Calculations;
using Calculator2Henry.Mocking;
using OfficeOpenXml;

namespace ReelerGameCalculator.Calculations
{
    public class RtpMain
    {
        public static void CalcRtpMain(EngineConfiguration engine)
        {
            string WildSymbolName = string.Empty;
            List<string> noneLineSymbol = new List<string>();
            foreach (var entry in engine.Symbols)
            {
                if (entry.Value.IsWild)
                {
                    WildSymbolName = entry.Key;
                }

                if (entry.Value.IsWild == false && entry.Value.IsWildSubstitutable == false)
                {
                    noneLineSymbol.Add(entry.Key);
                }
            }

            var displays = engine.Displays;

            foreach (var display in displays)
            {
                RtpDisplay model = new RtpDisplay();
                model.DisplayName = display.Name;
                model.Columns = display.Columns;
                model.Rows = display.Rows;

                var symbolSets = display.SymbolSets;
                foreach (var symbolSet in symbolSets)
                {
                    var name = symbolSet.Key;
                    var symbolCounts = symbolSet.Value;
                    int nonLinePaySymbolCount = 0;

                    model.SymbolWeightsPerReel.Add(symbolSet.Value);
                    foreach (var entry in symbolSet.Value)
                    {
                        if (noneLineSymbol.Contains(entry.Key))
                        {
                            nonLinePaySymbolCount += entry.Value;
                        }
                    }

                    model.WildName = WildSymbolName;
                    model.SymbolLengthPerReel.Add(symbolCounts.Sum(x => x.Value)); 
                    
                    model.NumNoLinePaySymbolsPerReel.Add(nonLinePaySymbolCount);
                }

                model.TotalCombinations = model.SymbolLengthPerReel.Aggregate(1, (acc, x) => acc * x);

                CalcRtpOnDisplay(engine, model);
            }
        }

        private static void CalcRtpOnDisplay(EngineConfiguration engine, RtpDisplay modelDisplayRtps)
        {
            modelDisplayRtps.NumLines = engine.NumLines;
            modelDisplayRtps.BetMultiplier = engine.Bet.Multiplier;
            
            var paytable = engine.PayTables["Default"];
            var paytableLineGroup = paytable[0];
            foreach (var configLinePay in paytableLineGroup)
            {
                var currentLineRtp = new RtpPayline(modelDisplayRtps, engine, configLinePay);
                currentLineRtp.Calculate();

                modelDisplayRtps.LineRtps.Add(configLinePay.Name, currentLineRtp);
                modelDisplayRtps.TotalLineRtp += currentLineRtp.Rtp1C;
            }

            var paytableScatterGroup = paytable[1];
            foreach (var configScatterPay in  paytableScatterGroup)
            {
                var currentScatterPay = new RtpScatterPay(modelDisplayRtps, engine, configScatterPay);
                currentScatterPay.Calculate();

                modelDisplayRtps.ScatterRtps.Add(configScatterPay.Name, currentScatterPay);
                modelDisplayRtps.TotalScatterRtp += currentScatterPay.Rtp1C;
            }
            modelDisplayRtps.TotalRtp = modelDisplayRtps.TotalLineRtp + modelDisplayRtps.TotalScatterRtp;

            #region
            int currentRow = 1;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Paytable RTP");
                worksheet.Cells[currentRow, 1].Value = "Items";
                worksheet.Cells[currentRow, 2].Value = "Rtp";
                currentRow++;

                foreach (var entry in modelDisplayRtps.LineRtps)
                {
                    worksheet.Cells[currentRow, 1].Value = entry.Key;
                    worksheet.Cells[currentRow, 2].Value = entry.Value.Rtp1C;
                    worksheet.Cells[currentRow, 2].Style.Numberformat.Format = "0.0000%";
                    currentRow++;
                }

                foreach (var entry in modelDisplayRtps.ScatterRtps)
                {
                    worksheet.Cells[currentRow, 1].Value = entry.Key;
                    worksheet.Cells[currentRow, 2].Value = entry.Value.Rtp1C;
                    worksheet.Cells[currentRow, 2].Style.Numberformat.Format = "0.0000%";
                    currentRow++;
                }

                string fileOutput = $"{OutputDefs.OutputFile}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.xlsx";
                File.WriteAllBytes(fileOutput, package.GetAsByteArray());
            }
            #endregion
        }
    }
}
