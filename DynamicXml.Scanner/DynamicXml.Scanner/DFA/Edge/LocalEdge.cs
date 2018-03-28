using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicXml.Scanner.DFA.Edge
{
    using State;

    public class LocalEdge : IEdge
    {
        private readonly Func<char[], bool> _transitionFunction;

        public LocalEdge(Func<char[], bool> transitionFunction)
        {
            _transitionFunction = transitionFunction ?? throw new ArgumentNullException(nameof(transitionFunction));
        }

        public bool Transition(char[] buffer)
        {
            return _transitionFunction(buffer);
        }
    }
}
