using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicXml.Scanner
{
    using Token;

    public class XmlScanner
    {
        private string _xmlData;

        public ScannedToken NextScannedToken { private set; get; }

        public XmlScanner(string xmlData)
        {
            if (xmlData != null) _xmlData = xmlData;
        }

        public void AdvanceTokenBuffer(TokenType specifiedToken = TokenType.Unspecified)
        {
            if (_xmlData.Length == 0)
            {
                NextScannedToken = new ScannedToken(TokenType.Eof, string.Empty);
                return;
            }

            if (specifiedToken == TokenType.Unspecified)
                AdvanceBufferForUnspecifiedTokenType();
            else
                AdvanceBufferForSpecifiedTokenType(specifiedToken);
        }

        #region Private Helper Methods

        private void AdvanceBufferForUnspecifiedTokenType()
        {
            foreach (var tokenRegex in TokenRegexLookup.Map)
            {
                var match = tokenRegex.Value.Match(_xmlData);

                if (!match.Success || match.Index != 0) continue;

                _xmlData = _xmlData.Remove(0, match.Length);
                NextScannedToken = new ScannedToken(tokenRegex.Key, match.Value);
                return;
            }

            NextScannedToken = new ScannedToken(TokenType.Undefined, null);
        }

        private void AdvanceBufferForSpecifiedTokenType(TokenType type)
        {
            var match = TokenRegexLookup.Map[type].Match(_xmlData);

            if (match.Success && match.Index == 0)
            {
                _xmlData = _xmlData.Remove(0, match.Length);
                NextScannedToken = new ScannedToken(type, match.Value);
            }

            else
                NextScannedToken = new ScannedToken(TokenType.Undefined, null);
        }

        #endregion
    }
}