namespace DynamicXml.Scanner.Scanner
{
    #region Imports

    using System;
    using Lexeme;
    using LexemeReader;

    #endregion

    public class XmlScanner : IScanner
    {
        private readonly ILexemeReader _lexemeReader;

        public XmlScanner(ILexemeReader injectedLexemeReader)
        {
            _lexemeReader = injectedLexemeReader ?? throw new ArgumentNullException(nameof(injectedLexemeReader));
        }

        public DynamicXml.Scanner.Lexeme.Lexeme NextLexeme { private set; get; }

        public void AdvanceLexemeBuffer(LexemeType specifiedLexeme = LexemeType.Unspecified)
        {
            NextLexeme = _lexemeReader.GetNextLexemeFromBuffer(specifiedLexeme);
        }
    }
}
