using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicXml.Scanner.TokenProvider
{
    using Regex;
    using Token;

    public class RegexTokenReader : ITokenReader
    {
        private string _buffer;

        public RegexTokenReader(string inputXml)
        {
            _buffer = inputXml;
        }

        public ScannedToken GetNextTokenFromBuffer(TokenType specifiedToken = TokenType.Unspecified)
        {
            if (_buffer.Length == 0)
                return new ScannedToken(TokenType.Eof, string.Empty);

            if (specifiedToken == TokenType.Unspecified)
                return AdvanceBufferForUnspecifiedTokenType();

            return AdvanceBufferForSpecifiedTokenType(specifiedToken);
        }

        #region Private Helper Methods

        private ScannedToken AdvanceBufferForUnspecifiedTokenType()
        {
            foreach (var tokenRegex in TokenRegexLookup.Map)
            {
                var match = tokenRegex.Value.Match(_buffer);

                if (!match.Success || match.Index != 0) continue;

                _buffer = _buffer.Remove(0, match.Length);
                return new ScannedToken(tokenRegex.Key, match.Value);
            }

            return new ScannedToken(TokenType.Undefined, null);
        }

        private ScannedToken AdvanceBufferForSpecifiedTokenType(TokenType type)
        {
            var match = TokenRegexLookup.Map[type].Match(_buffer);

            if (match.Success && match.Index == 0)
            {
                _buffer = _buffer.Remove(0, match.Length);
                return new ScannedToken(type, match.Value);
            }

            return new ScannedToken(TokenType.Undefined, null);
        }

        #endregion
    }
}
