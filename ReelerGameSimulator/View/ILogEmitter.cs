using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelerGameSimulator.View
{
    public interface ILogEmitter
    {
        string Emit();

        void ToConsole();
    }
}
