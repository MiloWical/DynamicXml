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

        public void AdvanceTokenBuffer()
        {
            if (_xmlData.Length == 0)
            {
                NextScannedToken = new ScannedToken(TokenType.Eof, string.Empty);
                return;
            }

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
    }
}
