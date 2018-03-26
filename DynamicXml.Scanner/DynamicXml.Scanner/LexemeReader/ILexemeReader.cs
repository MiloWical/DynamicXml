namespace DynamicXml.Scanner.LexemeReader
{
    #region Imports

    using Lexeme;

    #endregion

    public interface ILexemeReader
    {
        Lexeme GetNextLexemeFromBuffer(LexemeType specifiedLexeme);
    }
}
