namespace Calculator2Henry
{
    internal class InputDefs
    {
        internal const string InputFile = "data//Inputs.xlsx";
        internal const int InputSheetMain = 0;
        internal const int InputSheetReeler = 1;

        #region input symbol defs
        internal const int SymbolStartRowIndex = 2;
        internal const int SymbolEndRowIndex = 16;
        internal const int SymbolNameColIndex = 1;
        internal const int SymbolIdColIndex = 2;
        internal const int IsWildSymbolColIndex = 3;
        internal const int IsWildSubstitutableColIndex = 4;
        internal const int IsScatterColIndex = 5;
        #endregion

        #region Display 1
        #endregion 
    }

    internal class OutputDefs
    {
        internal const string OutputFile = "data//Outputs";
    }
}
