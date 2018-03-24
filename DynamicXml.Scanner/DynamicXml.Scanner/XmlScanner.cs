namespace DynamicXml.Scanner
{
    #region Imports

    using System;
    using Token;
    using TokenProvider;

    #endregion

    public class XmlScanner
    {
        private readonly ITokenReader _tokenReader;

        public XmlScanner(ITokenReader injectedTokenReader)
        {
            _tokenReader = injectedTokenReader ?? throw new ArgumentNullException(nameof(injectedTokenReader));
        }

        public ScannedToken NextScannedToken { private set; get; }

        public void AdvanceTokenBuffer(TokenType specifiedToken = TokenType.Unspecified)
        {
            NextScannedToken = _tokenReader.GetNextTokenFromBuffer(specifiedToken);
        }
    }
}
