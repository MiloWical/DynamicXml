namespace DynamicXml.Scanner.Scanner
{
    using Lexeme;

    public interface IScanner
    {
        DynamicXml.Scanner.Lexeme.Lexeme NextLexeme { get; }
        void AdvanceLexemeBuffer(LexemeType specifiedLexeme = LexemeType.Unspecified);
    }
}