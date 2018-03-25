using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicXml.Scanner.DFA.Edge
{
    using State;

    public interface IEdge
    {
        IState Transition(char[] buffer);
    }
}
