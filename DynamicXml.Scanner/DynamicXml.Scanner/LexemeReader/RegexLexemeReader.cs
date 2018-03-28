namespace DynamicXml.Scanner.LexemeReader
{
    using Lexeme;
    using Regex;

    public class RegexLexemeReader : ILexemeReader
    {
        private string _buffer;

        public RegexLexemeReader(string inputXml)
        {
            _buffer = inputXml;
        }

        public Lexeme GetNextLexemeFromBuffer(LexemeType specifiedLexeme = LexemeType.Unspecified)
        {
            if (_buffer.Length == 0)
                return new Lexeme(LexemeType.Eof, string.Empty);

            if (specifiedLexeme == LexemeType.Unspecified)
                return AdvanceBufferForUnspecifiedLexemeType();

            return AdvanceBufferForSpecifiedLexemeType(specifiedLexeme);
        }

        #region Private Helper Methods

        private Lexeme AdvanceBufferForUnspecifiedLexemeType()
        {
            foreach (var lexemeRegex in LexemeRegexLookup.Map)
            {
                var match = lexemeRegex.Value.Match(_buffer);

                if (!match.Success || match.Index != 0) continue;

                _buffer = _buffer.Remove(0, match.Length);
                return new Lexeme(lexemeRegex.Key, match.Value);
            }

            return new Lexeme(LexemeType.Undefined, null);
        }

        private Lexeme AdvanceBufferForSpecifiedLexemeType(LexemeType type)
        {
            var match = LexemeRegexLookup.Map[type].Match(_buffer);

            if (!match.Success || match.Index != 0) return new Lexeme(LexemeType.Undefined, null);

            _buffer = _buffer.Remove(0, match.Length);
            return new Lexeme(type, match.Value);
        }

        #endregion
    }
}
