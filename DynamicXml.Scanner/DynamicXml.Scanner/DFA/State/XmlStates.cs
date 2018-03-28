using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicXml.Scanner.DFA.State
{
    using Lexeme;

    public static class XmlStates
    {
        public static IState EofState = new TerminalState(LexemeType.Eof);
    }
}
