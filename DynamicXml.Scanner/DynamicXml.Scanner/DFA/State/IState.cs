using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicXml.Scanner.DFA.State
{
    public interface IState
    {
        IState TransitionToNextState(char[] buffer, ICollection<char> currentLexeme);
    }
}
