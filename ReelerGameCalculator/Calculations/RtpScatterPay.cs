using Calculator2Henry.Mocking;
using System.Diagnostics;

namespace Calculator2Henry.Calculations
{
    public class RtpScatterPay
    {
        public IReadOnlyList<IReadOnlyList<bool>> Pattern5ScattersOnReels { get; } = new List<List<bool>>() {
                new List<bool> { true, true, true, true, true },
        };
        public IReadOnlyList<IReadOnlyList<bool>> Pattern4ScattersOnReels { get; } = new List<List<bool>>() {
                new List<bool> { true, true, true, true, false },
                new List<bool> { true, true, true, false, true },
                new List<bool> { true, true, false, true, true },
                new List<bool> { true, false, true, true, true },
                new List<bool> { false, true, true, true, true }
        };
        public IReadOnlyList<IReadOnlyList<bool>> Pattern3ScattersOnReels { get; } = new List<List<bool>>() {
                new List<bool> { true, true, true, false, false },
                new List<bool> { true, true, false, true, false  },
                new List<bool> { true, false, true, true, false },
                new List<bool> { false, true, true, true, false  },

                new List<bool> { true, true, false, false, true  },
                new List<bool> { true, false, true, false, true },
                new List<bool> { false, true, true, false, true  },

                new List<bool> { true, false, false, true, true },
                new List<bool> { false, true, false, true, true  },

                new List<bool> { false, false, true, true, true  },
        };

        public RtpDisplay HeadModel { get; }
        public EngineConfiguration Engine { get; }
        public PayTableEntryConfig ConfigScatterPay { get; }

        public string FullPrizeName { get; }
        public string SymbolName { get; }
        public int SymbolCount { get; }

        public int Hits { get; private set; }
        public decimal HitRate { get; private set; }
        public decimal TotalHitRate { get; private set; }
        public decimal Rtp { get; private set; }
        public decimal Rtp1C { get; private set; }

        public RtpScatterPay(RtpDisplay modelDisplayRtps, EngineConfiguration engine, PayTableEntryConfig configScatterPay)
        {
            HeadModel = modelDisplayRtps;
            Engine = engine;
            ConfigScatterPay = configScatterPay;

            FullPrizeName = ConfigScatterPay.Name;
            string[] parts = configScatterPay.Name.Split('x');
            SymbolName = parts[1];
            SymbolCount = int.Parse(parts[0]);
        }

        internal void Calculate()
        {
            IReadOnlyList<IReadOnlyList<bool>> patternAtSymbolCount = null;
            switch (SymbolCount)
            {
                case 5:
                    patternAtSymbolCount = Pattern5ScattersOnReels;
                    break;

                case 4:
                    patternAtSymbolCount = Pattern4ScattersOnReels;
                    break;

                case 3:
                    patternAtSymbolCount = Pattern3ScattersOnReels;
                    break;

                default:
                    throw new NotImplementedException($"Unknown symbol count {SymbolCount} in line {ConfigScatterPay.Name}");
            }

            for (int i = 0; i < patternAtSymbolCount.Count; i++)
            {
                var currentPattern = patternAtSymbolCount[i];
                List<int> currentPatternHitsOnReels = new List<int>();
                for (int j = 0; j < HeadModel.Columns; j++)
                {
                    int currentReelHits = GetNumScatterHitsOnReel(j);
                    if (currentPattern[j] == true)
                    {
                        currentPatternHitsOnReels.Add(currentReelHits);
                    }
                    else
                    {
                        currentPatternHitsOnReels.Add(HeadModel.SymbolLengthPerReel[j] - currentReelHits);
                    }
                }

                int currentPatternHits = currentPatternHitsOnReels.Aggregate(1, (acc, x) => acc * x);
                decimal currentPatternHitRate = currentPatternHits / (decimal)HeadModel.TotalCombinations;
                decimal currentPatternTotalHitRate = currentPatternHitRate;
                decimal currentPatternRtp = currentPatternTotalHitRate * ConfigScatterPay.Payout;
                decimal currentPatternRtp1C = currentPatternRtp / HeadModel.BetMultiplier;

                Hits += currentPatternHits;
                HitRate += currentPatternHitRate;
                TotalHitRate += currentPatternTotalHitRate;
                Rtp += currentPatternRtp;
                Rtp1C += currentPatternRtp1C;
            }

            Debug.WriteLine($"{FullPrizeName}, RTP1c = {Rtp1C}");
        }

        private int GetNumScatterHitsOnReel(int reelIndex)
        {
            var reelSymbolWeights = HeadModel.SymbolWeightsPerReel[reelIndex];
            return reelSymbolWeights.TryGetValue(SymbolName, out int count) ? count * HeadModel.Rows : 0;
        }
    }
}
