using ReelerGameSimulator.Config.ReadOnlyData;
using ReelerGameSimulator.Rng;
using System;

namespace ReelerGameSimulator.Logic.Model
{
    public class Display
    {
        public string Name { get; private set; } = "Default";
        public int Columns { get; private set; } = 5;
        public int Rows { get; private set; } = 3;
        public List<DisplayItem> Symbols { get; private set; } = new List<DisplayItem>();

        public DisplayItem this[int index] => Symbols[index];

        public DisplayItem this[int column, int row] => Symbols[this.Columns * row + column];

        public Display()
        {
        }

        public Display(DisplayConfig displayConfig)
        {
            Name = displayConfig.Name;
            Columns = displayConfig.Columns;
            Rows = displayConfig.Rows;

            Symbols = new List<DisplayItem>();
            int total = Columns * Rows;
            for (int index = 0; index < total; index++)
            {
                int column = index % Columns;
                int row = index / Columns;
                Symbols.Add(new DisplayItem(index, column, row, new SymbolConfig()));
            }
        }

        internal string GetSymbolsString()
        {
            throw new NotImplementedException();
        }

        internal string GetSymbolGrid()
        {
            throw new NotImplementedException();
        }
    }

    public class DisplayItem
    {
        public int Index { get; }

        public int Column { get; }

        public int Row { get; }

        public SymbolConfig Symbol { get; set; }

        internal DisplayItem(int index, int column, int row, SymbolConfig symbol)
        {
            Index = index;
            Column = column;
            Row = row;
            Symbol = symbol;
        }
    }
}
