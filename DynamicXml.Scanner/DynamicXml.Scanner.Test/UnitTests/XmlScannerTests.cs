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

                if (scanner.NextScannedToken.Type == TokenType.Identifier)
                    Assert.AreEqual(expectedIdentifiers[expectedIdentifierIndex++], scanner.NextScannedToken.Literal);
            }
        }

        [TestMethod]
        public void TagParsingTest()
        {
            const string testString =
                "<Parent attrib=\"New Attribute\">\n\t<Child/>\n\t<Child>Some info</Child><Child id='2'/>\n</Parent>";

            var expectedTokens = new[]
            {
                TokenType.LessThanSymbol,
                TokenType.Identifier,
                TokenType.WhitespaceSymbol,
                TokenType.Identifier,
                TokenType.EqualSymbol,
                TokenType.DoubleQuoteSymbol,
                TokenType.Data,
                TokenType.DoubleQuoteSymbol,
                TokenType.GreaterThanSymbol,
                TokenType.WhitespaceSymbol,
                TokenType.LessThanSymbol,
                TokenType.Identifier,
                TokenType.SlashSymbol,
                TokenType.GreaterThanSymbol,
                TokenType.WhitespaceSymbol,
                TokenType.LessThanSymbol,
                TokenType.Identifier,
                TokenType.GreaterThanSymbol,
                TokenType.Data,
                TokenType.LessThanSymbol,
                TokenType.SlashSymbol,
                TokenType.Identifier,
                TokenType.GreaterThanSymbol,
                TokenType.WhitespaceSymbol, //This is optional whitespace
                TokenType.LessThanSymbol,
                TokenType.Identifier,
                TokenType.WhitespaceSymbol,
                TokenType.Identifier,
                TokenType.EqualSymbol,
                TokenType.SingleQuoteSymbol,
                TokenType.Data,
                TokenType.SingleQuoteSymbol,
                TokenType.SlashSymbol,
                TokenType.GreaterThanSymbol,
                TokenType.WhitespaceSymbol,
                TokenType.LessThanSymbol,
                TokenType.SlashSymbol,
                TokenType.Identifier,
                TokenType.GreaterThanSymbol,
                TokenType.Eof
            };

            var expectedLiterals = new[]
            {
                "Parent",
                "attrib",
                "New Attribute",
                "Child",
                "Child",
                "Some info",
                "Child",
                "Child",
                "id",
                "2",
                "Parent"
            };

            var expectedIdentifierIndex = 0;

            var scanner = new XmlScanner(testString);

            foreach (var tokenType in expectedTokens)
            {
                scanner.AdvanceTokenBuffer(tokenType);
                Assert.AreEqual(tokenType, scanner.NextScannedToken.Type);

                if (scanner.NextScannedToken.Type == TokenType.Identifier ||
                    scanner.NextScannedToken.Type == TokenType.Data)
                    Assert.AreEqual(expectedLiterals[expectedIdentifierIndex++], scanner.NextScannedToken.Literal);
            }
        }

        [TestMethod]
        public void CommentParsingTest()
        {
            const string testString =
                "<!--Comment--><tag><!-- New Comment with <tag> --></tag>";

            var expectedTokens = new[]
            {
                TokenType.Comment,
                TokenType.LessThanSymbol,
                TokenType.Identifier,
                TokenType.GreaterThanSymbol,
                TokenType.Comment,
                TokenType.LessThanSymbol,
                TokenType.SlashSymbol,
                TokenType.Identifier,
                TokenType.GreaterThanSymbol
            };

            var scanner = new XmlScanner(testString);

            foreach (var tokenType in expectedTokens)
            {
                scanner.AdvanceTokenBuffer(tokenType);
                Assert.AreEqual(tokenType, scanner.NextScannedToken.Type);
            }
        }

        [TestMethod]
        public void CDataParsingTest()
        {
            const string testString =
                "<!--Comment with <![CDATA ]]> --><tag><![CDATA[New character\ndata with a <tag>]]></tag>";
                //"<!--Comment with <![CDATA ]]> -->";

            var expectedTokens = new[]
            {
                TokenType.Comment,
                TokenType.LessThanSymbol,
                TokenType.Identifier,
                TokenType.GreaterThanSymbol,
                TokenType.CData,
                TokenType.LessThanSymbol,
                TokenType.SlashSymbol,
                TokenType.Identifier,
                TokenType.GreaterThanSymbol
            };

            var scanner = new XmlScanner(testString);

            foreach (var tokenType in expectedTokens)
            {
                scanner.AdvanceTokenBuffer(tokenType);
                Assert.AreEqual(tokenType, scanner.NextScannedToken.Type);
            }
        }
    }
}
