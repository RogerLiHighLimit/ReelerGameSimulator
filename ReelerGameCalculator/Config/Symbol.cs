using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator2Henry.Mocking
{
    public class Symbol
    {
        public string Name { get; internal set; }
        public bool IsWild { get; internal set; }
        public bool IsWildSubstitutable { get; internal set; }
    }
}
