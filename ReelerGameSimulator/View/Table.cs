using System.Text;

namespace ReelerGameSimulator.View
{
    public enum Alignment
    {
        Left,
        Right,
        Centre
    }

    public class Table : ILogEmitter
    {
        private class Item
        {
            public string Data { get; }

            public int Length => Data.Length;

            public ConsoleColor? Colour { get; set; }

            public Alignment? Alignment { get; set; }

            public Item(string data)
            {
                Data = data;
                Colour = null;
                Alignment = null;
            }

            public Item(object data)
                : this(data.ToString())
            {
            }

            public override string ToString()
            {
                return Data;
            }
        }

        private readonly int[] _columnWidths;

        private int _maxColumnWidth;

        private readonly Item[] _headings;

        private readonly List<Item[]> _rows;

        private const char _title = '-';

        private const char _col = '|';

        private const char _row = '-';

        private const char _intersect = '+';

        private const char _none = ' ';

        public string Title { get; }

        public int Columns { get; }

        public uint ItemPadding { get; set; }

        public Alignment ItemAlignment { get; set; }

        public uint Indentation { get; set; }

        public bool UniformColumns { get; set; }

        private Table()
        {
            ItemPadding = 1u;
            ItemAlignment = Alignment.Left;
            Indentation = 0u;
            UniformColumns = true;
        }

        public Table(int columns, int rows = 0)
            : this()
        {
            if (columns <= 0 || rows < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            Title = string.Empty;
            Columns = columns;
            _columnWidths = new int[columns];
            _rows = new List<Item[]>(rows);
            _headings = null;
        }

        public Table(string title, int columns, int rows = 0)
            : this(columns, rows)
        {
            Title = title ?? string.Empty;
        }

        public Table(IReadOnlyList<string> headings, Alignment alignment = Alignment.Centre, int rows = 0)
            : this()
        {
            Title = string.Empty;
            Columns = headings.Count;
            _columnWidths = new int[headings.Count];
            _rows = new List<Item[]>(rows);
            _headings = new Item[headings.Count];
            for (int i = 0; i < headings.Count; i++)
            {
                Item item = new Item(headings[i])
                {
                    Alignment = alignment
                };
                _headings[i] = item;
                _columnWidths[i] = item.Length;
                if (item.Length > _maxColumnWidth)
                {
                    _maxColumnWidth = item.Length;
                }
            }
        }

        public Table(string title, IReadOnlyList<string> headings, Alignment alignment = Alignment.Centre, int rows = 0)
            : this(headings, alignment, rows)
        {
            Title = title ?? string.Empty;
        }

        public void AddRow(params object[] row)
        {
            if (row == null || row.Length != Columns)
            {
                throw new InvalidOperationException();
            }

            Item[] array = new Item[row.Length];
            for (int i = 0; i < row.Length; i++)
            {
                Item item = (array[i] = new Item(row[i]));
                if (item.Length > _columnWidths[i])
                {
                    _columnWidths[i] = item.Length;
                }

                if (item.Length > _maxColumnWidth)
                {
                    _maxColumnWidth = item.Length;
                }
            }

            _rows.Add(array);
        }

        public void AddRow(string[] row, ConsoleColor?[] colours)
        {
            if (row == null || row.Length != Columns)
            {
                throw new InvalidOperationException();
            }

            if (colours != null && colours.Length != Columns)
            {
                throw new InvalidOperationException();
            }

            Item[] array = new Item[row.Length];
            for (int i = 0; i < row.Length; i++)
            {
                Item item = new Item(row[i]);
                item.Colour = ((colours != null) ? colours[i] : null);
                array[i] = item;
                if (item.Length > _columnWidths[i])
                {
                    _columnWidths[i] = item.Length;
                }

                if (item.Length > _maxColumnWidth)
                {
                    _maxColumnWidth = item.Length;
                }
            }

            _rows.Add(array);
        }

        public void AddRow<T>(T[] objects, Func<T, string> converter = null, Func<T, ConsoleColor?> colour = null)
        {
            if (objects == null || objects.Length != Columns)
            {
                throw new InvalidOperationException();
            }

            Item[] array = new Item[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                T val = objects[i];
                Item item = ((converter == null) ? new Item(val) : new Item(converter(val)));
                if (colour != null)
                {
                    item.Colour = colour(val);
                }

                array[i] = item;
                if (item.Length > _columnWidths[i])
                {
                    _columnWidths[i] = item.Length;
                }

                if (item.Length > _maxColumnWidth)
                {
                    _maxColumnWidth = item.Length;
                }
            }

            _rows.Add(array);
        }

        public void SetColour(IReadOnlyList<int> indexes, ConsoleColor? colour)
        {
            if (indexes == null)
            {
                throw new ArgumentNullException("indexes");
            }

            for (int i = 0; i < indexes.Count; i++)
            {
                SetColour(indexes[i], colour);
            }
        }

        public void SetColour(int index, ConsoleColor? colour)
        {
            if (index < 0 || index >= _rows.Count * Columns)
            {
                throw new ArgumentOutOfRangeException();
            }

            int index2 = index / Columns;
            int num = index % Columns;
            _rows[index2][num].Colour = colour;
        }

        public void SetAlignment(int index, Alignment? alignment)
        {
            if (index < 0 || index >= _rows.Count * Columns)
            {
                throw new ArgumentOutOfRangeException();
            }

            int index2 = index / Columns;
            int num = index % Columns;
            _rows[index2][num].Alignment = alignment;
        }

        public void SetColumnAlignment(int column, Alignment? alignment)
        {
            if (column < 0 || column >= Columns)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < _rows.Count; i++)
            {
                _rows[i][column].Alignment = alignment;
            }
        }

        public void ClearRows()
        {
            _rows.Clear();
            _maxColumnWidth = 0;
            for (int i = 0; i < _columnWidths.Length; i++)
            {
                _columnWidths[i] = 0;
            }
        }

        public bool SetMinColumnWidth(int width)
        {
            if (width > 0 && width > _maxColumnWidth)
            {
                _maxColumnWidth = width;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int rowSpan = GetRowSpan();
            string indent = new string(' ', (int)Indentation);
            string padding = new string(' ', (int)ItemPadding);
            string value = FormatBorder(rowSpan, indent, padding);
            if (TryFormatTitle(Title, rowSpan, indent, out var format))
            {
                string value2 = FormatTitleBorder(rowSpan, indent);
                stringBuilder.AppendLine(value2);
                stringBuilder.AppendLine(format);
            }

            if (_headings != null && _headings.Length == Columns)
            {
                string value3 = FormatBorder(rowSpan, indent, padding, heading: true);
                stringBuilder.AppendLine(value3);
                stringBuilder.AppendLine(FormatRow(_headings, rowSpan, indent, padding));
                stringBuilder.AppendLine(value3);
            }
            else
            {
                stringBuilder.AppendLine(value);
            }

            for (int i = 0; i < _rows.Count; i++)
            {
                stringBuilder.AppendLine(FormatRow(_rows[i], rowSpan, indent, padding));
                stringBuilder.AppendLine(value);
            }

            return stringBuilder.ToString();
        }

        public void ToConsole()
        {
            int rowSpan = GetRowSpan();
            string indent = new string(' ', (int)Indentation);
            string padding = new string(' ', (int)ItemPadding);
            string value = FormatBorder(rowSpan, indent, padding);
            if (TryFormatTitle(Title, rowSpan, indent, out var format))
            {
                string value2 = FormatTitleBorder(rowSpan, indent);
                Console.WriteLine(value2);
                Console.WriteLine(format);
            }

            if (_headings != null && _headings.Length == Columns)
            {
                string value3 = FormatBorder(rowSpan, indent, padding, heading: true);
                Console.WriteLine(value3);
                FormatConsoleRow(_headings, indent, padding);
                Console.WriteLine(value3);
            }
            else
            {
                Console.WriteLine(value);
            }

            for (int i = 0; i < _rows.Count; i++)
            {
                FormatConsoleRow(_rows[i], indent, padding);
                Console.WriteLine(value);
            }
        }

        private int GetRowSpan()
        {
            int num = ((!UniformColumns) ? (_columnWidths.Sum() + (int)(ItemPadding * 2) * Columns) : ((_maxColumnWidth + (int)(ItemPadding * 2)) * Columns));
            return (int)Indentation + num + Columns + 1;
        }

        private static bool TryFormatTitle(string title, int size, string indent, out string format)
        {
            format = null;
            if (string.IsNullOrWhiteSpace(title))
            {
                return false;
            }

            if (title.Length > size - indent.Length)
            {
                format = $"{124} {title.Substring(0, size - indent.Length - 4)} {124}";
                return true;
            }

            format = $"{indent}{124} {AlignString(title, size - indent.Length - 4, Alignment.Centre)} {124}";
            return true;
        }

        private static string FormatTitleBorder(int size, string indent)
        {
            return $"{indent}{43}{new string('-', size - indent.Length - 2)}{43}";
        }

        private string FormatBorder(int size, string indent, string padding, bool heading = false)
        {
            StringBuilder sb = new StringBuilder(size);
            if (UniformColumns)
            {
                return FormatBorder(sb, '-', size, _maxColumnWidth, indent, padding);
            }

            return FormatBorder(sb, '-', _columnWidths, indent, padding);
        }

        private static string FormatBorder(StringBuilder sb, char ch, int size, int width, string indent, string padding)
        {
            string text = new string(ch, width + padding.Length * 2);
            sb.Append(indent);
            for (int i = indent.Length; i < size - 1; i += text.Length + 1)
            {
                sb.Append('+');
                sb.Append(text);
            }

            sb.Append('+');
            return sb.ToString();
        }

        private static string FormatBorder(StringBuilder sb, char ch, int[] widths, string indent, string padding)
        {
            sb.Append(indent);
            for (int i = 0; i < widths.Length; i++)
            {
                string value = new string(ch, widths[i] + padding.Length * 2);
                sb.Append('+');
                sb.Append(value);
            }

            sb.Append('+');
            return sb.ToString();
        }

        private void FormatConsoleRow(Item[] items, string indent, string padding)
        {
            Console.Write(indent);
            for (int i = 0; i < items.Length; i++)
            {
                Console.Write("|" + padding);
                Item item = items[i];
                if (item.Colour.HasValue)
                {
                    Console.ForegroundColor = item.Colour.Value;
                }

                int width = _maxColumnWidth;
                if (!UniformColumns)
                {
                    width = _columnWidths[i];
                }

                Console.Write(AlignItem(item, width));
                Console.ResetColor();
                Console.Write(padding ?? "");
            }

            Console.WriteLine('|');
        }

        private string FormatRow(Item[] items, int size, string indent, string padding)
        {
            StringBuilder stringBuilder = new StringBuilder(size);
            stringBuilder.Append(indent);
            for (int i = 0; i < items.Length; i++)
            {
                int width = _maxColumnWidth;
                if (!UniformColumns)
                {
                    width = _columnWidths[i];
                }

                string value = AlignItem(items[i], width);
                StringBuilder stringBuilder2 = stringBuilder;
                StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(0, 4, stringBuilder2);
                handler.AppendFormatted('|');
                handler.AppendFormatted(padding);
                handler.AppendFormatted(value);
                handler.AppendFormatted(padding);
                stringBuilder2.Append(ref handler);
            }

            stringBuilder.Append('|');
            return stringBuilder.ToString();
        }

        private string AlignItem(Item item, int width)
        {
            return AlignString(item.Data, width, item.Alignment ?? ItemAlignment);
        }

        private static string AlignString(string s, int width, Alignment alignment)
        {
            return alignment switch
            {
                Alignment.Left => s.PadRight(width),
                Alignment.Right => s.PadLeft(width),
                Alignment.Centre => PadCentre(s, width),
                _ => s.PadRight(width),
            };
        }

        private static string PadCentre(string s, int width)
        {
            if (s.Length > width)
            {
                return s;
            }

            string text = new string(' ', (width - s.Length) / 2);
            string text2 = new string(' ', width - s.Length - text.Length);
            return text + s + text2;
        }

        public string Emit()
        {
            return ToString();
        }
    }
}
