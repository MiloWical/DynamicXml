namespace DynamicXml.Scanner.Test.UnitTests.TokenProvider
{
    #region Imports

    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scanner.TokenProvider;
    using Token;

    #endregion

    /// <summary>
    ///     Summary description for RegexTokenReaderTests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegexTokenReaderTests
    {
        [TestMethod]
        public void RegexTokenReaderSymbolTokenReadingTest()
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

            var reader = new RegexTokenReader(testString);

            foreach (var tokenType in expectedTokens)
            {
                var token = reader.GetNextTokenFromBuffer();
                Assert.AreEqual(tokenType, token.Type);
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

            var reader = new RegexTokenReader(testString);

            foreach (var tokenType in expectedTokens)
            {
                var token = reader.GetNextTokenFromBuffer();
                Assert.AreEqual(tokenType, token.Type);

                if (token.Type == TokenType.Identifier)
                    Assert.AreEqual(expectedIdentifiers[expectedIdentifierIndex++], token.Literal);
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

            var reader = new RegexTokenReader(testString);

            foreach (var tokenType in expectedTokens)
            {
                var token = reader.GetNextTokenFromBuffer(tokenType);
                Assert.AreEqual(tokenType, token.Type);

                if (token.Type == TokenType.Identifier ||
                    token.Type == TokenType.Data)
                    Assert.AreEqual(expectedLiterals[expectedIdentifierIndex++], token.Literal);
            }
        }

        [TestMethod]
        public void CommentParsingTest()
        {
            const string testString =
                "<!--Comment--><tag><!-- New Comment with <tag> and \t\n\t some whitespace --></tag>";

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

            var reader = new RegexTokenReader(testString);

            foreach (var tokenType in expectedTokens)
            {
                var token = reader.GetNextTokenFromBuffer(tokenType);
                Assert.AreEqual(tokenType, token.Type);
            }
        }

        [TestMethod]
        public void CDataParsingTest()
        {
            const string testString =
                "<!--Comment with <![CDATA ]]> --><tag><![CDATA[New character\ndata with a <tag>]]></tag>";

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

            var reader = new RegexTokenReader(testString);

            foreach (var tokenType in expectedTokens)
            {
                var token = reader.GetNextTokenFromBuffer(tokenType);
                Assert.AreEqual(tokenType, token.Type);
            }
        }
    }
}
