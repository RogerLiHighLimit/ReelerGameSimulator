using OfficeOpenXml;
using System.Xml.Linq;

namespace Calculator2Henry.Mocking
{
    public class EngineConfiguration
    {
        public Dictionary<string, SymbolConfig> Symbols { get; set; } = new Dictionary<string, SymbolConfig>();
        public List<DisplayConfig> Displays { get; } = new List<DisplayConfig>();
        public Dictionary<string, PayTableConfig> PayTables { get; internal set; } = new Dictionary<string, PayTableConfig>();
        public BetConfig Bet { get; } = new BetConfig();
        public int NumLines { get; internal set; } = 20;

        public static EngineConfiguration Load()
        {
            EngineConfiguration engine = new EngineConfiguration();

            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
            FileInfo file = new FileInfo(InputDefs.InputFile);
            using (var package = new ExcelPackage(file))
            {
                var worksheet0 = package.Workbook.Worksheets[InputDefs.InputSheetMain];
                LoadSymbolConfigs(worksheet0, engine);

                var worksheet1 = package.Workbook.Worksheets[InputDefs.InputSheetReeler]; 
                LoadDisplayConfigs(worksheet1, engine);
            }

            #region PayTables
            var defaultPaytable = new PayTableConfig();
            #region payline group
            PaytableLineGroup group0 = new PaytableLineGroup()
            {
                new PayTableEntryConfig() { Name = "5xWD", Payout = 300  },
                new PayTableEntryConfig() { Name = "4xWD", Payout = 25  },
                new PayTableEntryConfig() { Name = "3xWD", Payout = 4  },

                new PayTableEntryConfig() { Name = "5xH1", Payout = 75  },
                new PayTableEntryConfig() { Name = "4xH1", Payout = 25  },
                new PayTableEntryConfig() { Name = "3xH1", Payout = 4  },

                new PayTableEntryConfig() { Name = "5xH2", Payout = 50  },
                new PayTableEntryConfig() { Name = "4xH2", Payout = 12  },
                new PayTableEntryConfig() { Name = "3xH2", Payout = 2  },

                new PayTableEntryConfig() { Name = "5xH3", Payout = 50  },
                new PayTableEntryConfig() { Name = "4xH3", Payout = 12  },
                new PayTableEntryConfig() { Name = "3xH3", Payout = 2  },

                new PayTableEntryConfig() { Name = "5xH4", Payout = 50  },
                new PayTableEntryConfig() { Name = "4xH4", Payout = 12  },
                new PayTableEntryConfig() { Name = "3xH4", Payout = 2  },

                new PayTableEntryConfig() { Name = "5xL1", Payout = 40  },
                new PayTableEntryConfig() { Name = "4xL1", Payout = 7  },
                new PayTableEntryConfig() { Name = "3xL1", Payout = 2  },

                new PayTableEntryConfig() { Name = "5xL2", Payout = 25  },
                new PayTableEntryConfig() { Name = "4xL2", Payout = 5  },
                new PayTableEntryConfig() { Name = "3xL2", Payout = 2  },

                new PayTableEntryConfig() { Name = "5xL3", Payout = 25  },
                new PayTableEntryConfig() { Name = "4xL3", Payout = 5  },
                new PayTableEntryConfig() { Name = "3xL3", Payout = 2  },

                new PayTableEntryConfig() { Name = "5xL4", Payout = 25  },
                new PayTableEntryConfig() { Name = "4xL4", Payout = 5  },
                new PayTableEntryConfig() { Name = "3xL4", Payout = 2  },

                new PayTableEntryConfig() { Name = "5xL5", Payout = 25  },
                new PayTableEntryConfig() { Name = "4xL5", Payout = 5  },
                new PayTableEntryConfig() { Name = "3xL5", Payout = 2  },
            };
            defaultPaytable.Add(group0);
            #endregion

            #region scatter group
            PaytableLineGroup group1 = new PaytableLineGroup()
            {
                new PayTableEntryConfig() { Name = "5xSC", Payout = 50  },
                new PayTableEntryConfig() { Name = "4xSC", Payout = 5  },
                new PayTableEntryConfig() { Name = "3xSC", Payout = 1  },
            };
            defaultPaytable.Add(group1);
            #endregion

            engine.PayTables.Add("Default", defaultPaytable);
            #endregion

            return engine;
        }

        private static void LoadDisplayConfigs(ExcelWorksheet worksheet, EngineConfiguration engine)
        {
            var displayConfig = new DisplayConfig();
            engine.Displays.Add(displayConfig);

            displayConfig.Name = worksheet.Cells[1, 1].Text;
            List<string> symbolNames = engine.Symbols.Values.Select(x => x.Name).ToList();

            for (int reelIndex = 0; reelIndex < 5; reelIndex++)
            {
                LoadReelDistribution(worksheet, displayConfig, symbolNames, reelIndex);
            }
        }

        private static void LoadSymbolConfigs(ExcelWorksheet worksheet0, EngineConfiguration engine)
        {
            for (int r = InputDefs.SymbolStartRowIndex; r <= InputDefs.SymbolEndRowIndex; r++)
            {
                var symbol = new SymbolConfig
                {
                    Name = worksheet0.Cells[r, InputDefs.SymbolNameColIndex].Text,
                    Id = worksheet0.Cells[r, InputDefs.SymbolIdColIndex].GetValue<int>(),
                    IsWild = worksheet0.Cells[r, InputDefs.IsWildSymbolColIndex].GetValue<bool>(),
                    IsWildSubstitutable = worksheet0.Cells[r, InputDefs.IsWildSubstitutableColIndex].GetValue<bool>(),
                    IsScatter = worksheet0.Cells[r, InputDefs.IsScatterColIndex].GetValue<bool>()
                };
                engine.Symbols.Add(symbol.Name, symbol);
            }
        }

        private static void LoadReelDistribution(ExcelWorksheet worksheet1, DisplayConfig display, List<string> symbolNames, int reelIndex)
        {
            int colIndex = reelIndex + 2;
            var reelStripName = worksheet1.Cells[2, colIndex].Text;

            Dictionary<string, int> symbolDistributionOnReel = new Dictionary<string, int>();
            display.SymbolSets.Add(reelStripName, symbolDistributionOnReel);
            int rowIndex = 3;
            for (int i = 0; i < symbolNames.Count; i++)
            {
                var symbolName = symbolNames[i];
                int symbolCount = worksheet1.Cells[rowIndex, colIndex].GetValue<int>(); 
                symbolDistributionOnReel.Add(symbolName, symbolCount);

                rowIndex++;
            }
        }
    }
}
