namespace DynamicXml.Scanner.DFA.State
{
    #region Imports

    using System.Collections.Generic;
    using Lexeme;

    #endregion

    public class TerminalState : IState
    {
        public TerminalState(LexemeType lexemeType)
        {
            Type = lexemeType;
        }

        public LexemeType Type { get; }

        public IState TransitionToNextState(char[] buffer, ICollection<char> currentLexeme)
        {
            return null;
        }
    }
}
