namespace DynamicXml.Scanner.DFA.State
{
    #region Imports

    using Lexeme;

    #endregion

    public class TerminalState : IState
    {
        public TerminalState(Lexeme lexeme)
        {
            Lexeme = lexeme;
        }

        public Lexeme Lexeme { get; }

        public IState TransitionToNextState(char[] buffer)
        {
            return null;
        }
    }
}
