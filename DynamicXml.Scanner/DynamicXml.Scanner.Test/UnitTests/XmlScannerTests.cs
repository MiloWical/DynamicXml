using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicXml.Scanner.Test.UnitTests
{
    using DynamicXml.Scanner.Token;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Summary description for XmlScannerTests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class XmlScannerTests
    {
        [TestMethod]
        public void XmlScannerSymbolTokenReadingTest()
        {
            const string testString = ":  \"'=/><?";

            var expectedTokens = new[]
            {
                TokenType.ColonSymbol,
                TokenType.WhitespaceSymbol,
                TokenType.DoubleQuoteSymbol,
                TokenType.SingleQuoteSymbol,
                TokenType.EqualSymbol,
                TokenType.SlashSymbol,
                TokenType.GreaterThanSymbol,
                TokenType.LessThanSymbol,
                TokenType.QuestionMarkSymbol,
                TokenType.Eof
            };

            var scanner = new XmlScanner(testString);

            foreach (var tokenType in expectedTokens)
            {
                scanner.AdvanceTokenBuffer();
                Assert.AreEqual(tokenType, scanner.NextScannedToken.Type);
            }
        }

        [TestMethod]
        public void XmlPrologTokenizingTest()
        {
            const string testString = "<?xml version=\"1.0\" encoding='UTF-8'?>";

            var expectedTokens = new[]
            {
                TokenType.LessThanSymbol,
                TokenType.QuestionMarkSymbol,
                TokenType.Identifier,
                TokenType.WhitespaceSymbol,
                TokenType.Identifier,
                TokenType.EqualSymbol,
                TokenType.DoubleQuoteSymbol,
                TokenType.Version,
                TokenType.DoubleQuoteSymbol,
                TokenType.WhitespaceSymbol,
                TokenType.Identifier,
                TokenType.EqualSymbol,
                TokenType.SingleQuoteSymbol,
                TokenType.Identifier,
                TokenType.SingleQuoteSymbol,
                TokenType.QuestionMarkSymbol,
                TokenType.GreaterThanSymbol,
                TokenType.Eof
            };

            var expectedIdentifiers = new[]
            {
                "xml",
                "version",
                "encoding",
                "UTF-8"
            };

            var expectedIdentifierIndex = 0;

            var scanner = new XmlScanner(testString);

            foreach (var tokenType in expectedTokens)
            {
                scanner.AdvanceTokenBuffer();
                Assert.AreEqual(tokenType, scanner.NextScannedToken.Type);

                if(scanner.NextScannedToken.Type == TokenType.Identifier)
                    Assert.AreEqual(expectedIdentifiers[expectedIdentifierIndex++], scanner.NextScannedToken.Literal);
            }
        }
    }
}
