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
            const string testString = ":  \"'=/><";

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
                TokenType.Eof
            };

            var scanner = new XmlScanner(testString);

            foreach (var tokenType in expectedTokens)
            {
                scanner.AdvanceTokenBuffer();
                Assert.AreEqual(tokenType, scanner.NextScannedToken.Type);
            }
        }
    }
}
