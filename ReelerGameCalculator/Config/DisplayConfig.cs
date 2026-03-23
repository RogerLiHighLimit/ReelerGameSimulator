using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator2Henry.Mocking
{
    public class DisplayConfig
    {
        public string Name { get; set; } = "Default";
        public int Columns { get; set; } = 5;
        public int Rows { get; set; } = 3;

        public Dictionary<string, Dictionary<string, int>> SymbolSets { get; } = new Dictionary<string, Dictionary<string, int>>();
    }
}
