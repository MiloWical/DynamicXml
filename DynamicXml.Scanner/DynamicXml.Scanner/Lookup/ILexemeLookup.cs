namespace DynamicXml.Scanner.Lookup
{
    using DFA.State;
    using Lexeme;

    public interface ILexemeLookup
    {
        IState this[LexemeType type] { get; }
    }
}
