using ReelerGameSimulator.Config;
using ReelerGameSimulator.Config.ReadOnlyData;
using ReelerGameSimulator.Logic.Model;

namespace ReelerGameSimulator.Logic
{
    internal class GameLogicUtility
    {
        internal static List<PayoutResult> MatchLineWins(GameState st, GameConfig config)
        {
            var eventConfig = st.EventConfig;
            var paytable = eventConfig.PaytableLines.Entries;

            List<PayoutResult> result = new List<PayoutResult>();

            var lineDefs = eventConfig.PayoutProcessorConfig.Lines;
            for (int lineId = 0; lineId < lineDefs.Count; lineId++)
            {
                var currentLineDef = lineDefs[lineId];
                List<SymbolConfig> currentLineSymbolConfigList = new List<SymbolConfig>();
                for (int pos = 0; pos < currentLineDef.Count; pos++)
                {
                    int currentIndex = currentLineDef[pos];
                    var currentSymbol = st.Display[currentIndex]; 
                    currentLineSymbolConfigList.Add(currentSymbol.Symbol);
                }

                #region first symbol initialization
                string FirstSymbolName = currentLineSymbolConfigList[0].Name;
                string LineSymbolsString = FirstSymbolName;
                int FirstSymbolDisplayIndex = currentLineDef[0];
                List<int> lineIndexes = new List<int>() { FirstSymbolDisplayIndex };
                int numWild = 0;
                #endregion

                if (FirstSymbolName == eventConfig.WildSymbol.Name)
                {
                    // line start with wilds
                    numWild++;

                    for (int pos = 1; pos < currentLineDef.Count; pos++)
                    {
                        int currentIndex = currentLineDef[pos];
                        lineIndexes.Add(currentIndex);

                        var currentSymbol = currentLineSymbolConfigList[pos];
                        if (FirstSymbolName == eventConfig.WildSymbol.Name && currentSymbol.Name == eventConfig.WildSymbol.Name)
                        {
                            // continue with wilds line
                            LineSymbolsString += "," + FirstSymbolName;
                            numWild++;
                        }
                        else if (currentSymbol.IsNoneLineSymbols)
                        {
                            // current symbol is none-line-pay, line discontinue
                            break;
                        }
                        else
                        {
                            // current symbol is none-wilds line symbol.
                            if (FirstSymbolName == eventConfig.WildSymbol.Name)
                            {
                                // first none-wild line symbol, wilds line convert to wilds substitute line
                                FirstSymbolName = currentSymbol.Name;
                                LineSymbolsString = FirstSymbolName;
                                for (int i = 1; i <= pos; i++)
                                {
                                    LineSymbolsString += "," + FirstSymbolName;
                                }
                            }
                            else
                            {
                                if (currentSymbol.Name == FirstSymbolName)
                                {
                                    LineSymbolsString += "," + FirstSymbolName;
                                }
                                else if (currentSymbol.Name == eventConfig.WildSymbol.Name)
                                {
                                    LineSymbolsString += "," + FirstSymbolName;
                                    numWild++;
                                }
                                else
                                {
                                    // line discontinue
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // line start with non-wilds
                    for (int pos = 1; pos < currentLineDef.Count; pos++)
                    {
                        int currentIndex = currentLineDef[pos];
                        lineIndexes.Add(currentIndex);

                        var currentSymbol = currentLineSymbolConfigList[pos];
                        if (currentSymbol.Name == FirstSymbolName)
                        {
                            LineSymbolsString += "," + FirstSymbolName;
                        }
                        else if (currentSymbol.Name == eventConfig.WildSymbol.Name)
                        {
                            LineSymbolsString += "," + FirstSymbolName;
                            numWild++;
                        }
                        else
                        {
                            // line discontinue
                            break;
                        }
                    }
                }

                if (paytable.ContainsKey(LineSymbolsString))
                {
                    PayoutResult newResult = new PayoutResult(paytable[LineSymbolsString], Config.Data.PayTableType.Payline, 1, lineId, string.Join(",", currentLineDef), lineIndexes, numWild);
                    result.Add(newResult); 
                }
            }

            return result;
        }
    }
}
