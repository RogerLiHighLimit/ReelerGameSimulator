using Calculator2Henry.Mocking;
using System.Diagnostics;

namespace Calculator2Henry.Calculations
{
    public class RtpDisplay
    {
        public int NumLines { get; internal set; }
        public int BetMultiplier { get; internal set; }

        public string DisplayName { get; internal set; }

        public int Columns { get; internal set; }
        public int Rows { get; internal set; }

        public List<Dictionary<string, int>> SymbolWeightsPerReel { get; } = new List<Dictionary<string, int>>();
        public List<int> SymbolLengthPerReel { get; } = new List<int>();

        public string WildName { get; internal set; }
        public List<int> NumNoLinePaySymbolsPerReel { get; } = new List<int>();

        public int TotalCombinations { get; set; }

        public Dictionary<string, RtpPayline> LineRtps { get; } = new Dictionary<string, RtpPayline>();
        public Dictionary<string, RtpScatterPay> ScatterRtps { get; } = new Dictionary<string, RtpScatterPay>();

        public decimal TotalLineRtp { get; set; }
        public decimal TotalScatterRtp { get; set; }
        public decimal TotalRtp { get; set; }

        public RtpDisplay()
        {
           
        }
    }

    public class RtpPayline
    {
        public RtpDisplay HeadModel { get; }

        public PayTableEntryConfig ConfigLinePay { get; }
        public string FullPrizeName => ConfigLinePay.Name;
        public string SymbolName { get; }
        public int  SymbolCount { get; }
        public bool IsWildPay { get; } = false;

        public List<int> NumberHitsOnReels { get; } = new List<int>();
        public List<int> NumberHitsWildsOnlyOnReels { get; } = new List<int>();

        public int Hits { get; private set; }
        public decimal HitRate { get; private set; }
        public decimal TotalHitRate { get; private set; }
        public decimal Rtp { get; private set; }
        public decimal Rtp1C { get; private set; }

        public RtpPayline(RtpDisplay modelDisplayRtps, EngineConfiguration engine, PayTableEntryConfig ConfigLinePay)
        {
            HeadModel = modelDisplayRtps;
            this.ConfigLinePay = ConfigLinePay;

            string[] parts = ConfigLinePay.Name.Split('x');
            SymbolName = parts[1];
            SymbolCount = int.Parse(parts[0]);
            IsWildPay = engine.Symbols[SymbolName].IsWild;
        }

        public void Calculate()
        {
            if (FullPrizeName == "5xWD")
            {
                bool test = true;
            }

            if (IsWildPay)
            {
                GetWildPayHits();
            }
            else
            {
                GetWildSubstitutablePayHits();
            }

            Hits = NumberHitsOnReels.Aggregate(1, (acc, x) => acc * x);
            if (IsWildPay == false)
            {
                // hit adjustment
                int WildsHits = NumberHitsWildsOnlyOnReels.Aggregate(1, (acc, x) => acc * x);
                Hits -= WildsHits;
            }

            HitRate = Hits / (decimal)HeadModel.TotalCombinations;
            TotalHitRate = HitRate * HeadModel.NumLines;
            Rtp = TotalHitRate * ConfigLinePay.Payout;
            Rtp1C = Rtp / HeadModel.BetMultiplier;

            Debug.WriteLine($"{FullPrizeName}, RTP1c = {Rtp1C}");
        }

        private void GetWildPayHits()
        {
            switch (SymbolCount)
            {
                case 5:
                    for (int i = 0; i < SymbolCount; i++)
                    {
                        var symbolCounts = HeadModel.SymbolWeightsPerReel[i];
                        int numWilds = GetNumSymbolOnReel(symbolCounts, HeadModel.WildName);
                        NumberHitsOnReels.Add(numWilds);
                    }
                    break;

                case 4:
                    for (int i = 0; i < SymbolCount; i++)
                    {
                        var symbolCounts = HeadModel.SymbolWeightsPerReel[i];
                        int numWilds = GetNumSymbolOnReel(symbolCounts, HeadModel.WildName);
                        NumberHitsOnReels.Add(numWilds);
                    }

                    // fifth
                    NumberHitsOnReels.Add(HeadModel.NumNoLinePaySymbolsPerReel[SymbolCount]);
                    break;

                case 3:
                    for (int i = 0; i < SymbolCount; i++)
                    {
                        var symbolCounts = HeadModel.SymbolWeightsPerReel[i];
                        int numWilds = GetNumSymbolOnReel(symbolCounts, HeadModel.WildName);
                        NumberHitsOnReels.Add(numWilds);
                    }

                    NumberHitsOnReels.Add(HeadModel.NumNoLinePaySymbolsPerReel[SymbolCount]);
                    NumberHitsOnReels.Add(HeadModel.SymbolLengthPerReel[SymbolCount + 1]);
                    break;

                default:
                    throw new NotImplementedException($"Unknown symbol count {SymbolCount} in line {ConfigLinePay.Name}");
            }
        }

        private void GetWildSubstitutablePayHits()
        {
            for (int i = 0; i < SymbolCount; i++)
            {
                var reelSymbolWeights = HeadModel.SymbolWeightsPerReel[i];
                int numWilds = GetNumSymbolOnReel(reelSymbolWeights, HeadModel.WildName);
                int numLineSymbols = GetNumSymbolOnReel(reelSymbolWeights, SymbolName);
                NumberHitsOnReels.Add(numWilds + numLineSymbols);
            }

            switch (SymbolCount)
            {
                case 5:
                    #region compensation
                    for (int i = 0; i < HeadModel.SymbolWeightsPerReel.Count; i++)
                    {
                        var reelSymbolWeights = HeadModel.SymbolWeightsPerReel[i];
                        int numWilds = GetNumSymbolOnReel(reelSymbolWeights, HeadModel.WildName);
                        NumberHitsWildsOnlyOnReels.Add(numWilds);
                    }
                    #endregion
                    break;

                case 4:
                    // fifth
                    int fifth = HeadModel.SymbolWeightsPerReel.Count - 1;
                    var fifthReelSymbolWeights = HeadModel.SymbolWeightsPerReel[fifth];
                    var fifthReelLength = HeadModel.SymbolLengthPerReel[fifth];
                    var numWildsfifthReel = GetNumSymbolOnReel(fifthReelSymbolWeights, HeadModel.WildName);
                    var numSymbolfifthReel = GetNumSymbolOnReel(fifthReelSymbolWeights, SymbolName);
                    NumberHitsOnReels.Add(fifthReelLength - numWildsfifthReel - numSymbolfifthReel);

                    #region compensation
                    for (int i = 0; i < SymbolCount; i++)
                    {
                        var reelSymbolWeights = HeadModel.SymbolWeightsPerReel[i];
                        int numWilds = GetNumSymbolOnReel(reelSymbolWeights, HeadModel.WildName);
                        NumberHitsWildsOnlyOnReels.Add(numWilds);
                    }

                    // fifth
                    NumberHitsWildsOnlyOnReels.Add(fifthReelLength - numWildsfifthReel - numSymbolfifthReel);
                    #endregion
                    break;

                case 3:
                    // forth
                    int forth = SymbolCount;
                    var forthReelSymbolsWeights = HeadModel.SymbolWeightsPerReel[forth];
                    int forthReelLength = HeadModel.SymbolLengthPerReel[forth];
                    var numWildsforthReel = GetNumSymbolOnReel(forthReelSymbolsWeights, HeadModel.WildName);
                    var numSymbolforthhReel = GetNumSymbolOnReel(forthReelSymbolsWeights, SymbolName);
                    NumberHitsOnReels.Add(forthReelLength - numWildsforthReel - numSymbolforthhReel);
                    // fifth
                    NumberHitsOnReels.Add(HeadModel.SymbolLengthPerReel[SymbolCount + 1]);

                    #region compensation CurrentLineSymbolCountsListWithWilds
                    for (int i = 0; i < SymbolCount; i++)
                    {
                        var reelSymbolWeights = HeadModel.SymbolWeightsPerReel[i];
                        int numWilds = GetNumSymbolOnReel(reelSymbolWeights, HeadModel.WildName);
                        NumberHitsWildsOnlyOnReels.Add(numWilds);
                    }

                    // forth
                    NumberHitsWildsOnlyOnReels.Add(forthReelLength - numWildsforthReel - numSymbolforthhReel);
                    // fifth
                    NumberHitsWildsOnlyOnReels.Add(HeadModel.SymbolLengthPerReel[SymbolCount + 1]);
                    #endregion
                    break;

                default:
                    throw new NotImplementedException($"Unknown symbol count {SymbolCount} in line {ConfigLinePay.Name}");
            }
        }

        private static int GetNumSymbolOnReel(Dictionary<string, int> reelSymbolWeights, string symbolName)
        {
            return reelSymbolWeights.TryGetValue(symbolName, out int count) ? count : 0;
        }
    }
}
