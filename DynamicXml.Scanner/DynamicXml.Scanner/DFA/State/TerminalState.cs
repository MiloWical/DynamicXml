namespace DynamicXml.Scanner.DFA.State
{
    #region Imports

    using System.Collections.Generic;
    using Lexeme;

    #endregion

    public class TerminalState : IState
    {
        public LexemeType Type { get; }

        public string Name { get; }

        public TerminalState(LexemeType lexemeType, string name = null)
        {
            Type = lexemeType;
            Name = name;
        }

        public IState TransitionToNextState(char[] buffer, ICollection<char> currentLexeme)
        {
            return null;
        }
    }
}
