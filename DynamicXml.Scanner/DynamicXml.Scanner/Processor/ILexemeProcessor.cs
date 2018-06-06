namespace DynamicXml.Scanner.Processor
{
    using System.Collections.Generic;
    using DFA.State;
    using Lexeme;

    public interface ILexemeProcessor
    {
        IState this[LexemeType type] { get; }
        IState GetNextState(IState currentState, ICollection<char> currentLexeme);
    }
}
