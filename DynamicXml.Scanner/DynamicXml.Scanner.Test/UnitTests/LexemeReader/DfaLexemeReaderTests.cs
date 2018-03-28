namespace DynamicXml.Scanner.Test.UnitTests.LexemeReader
{
    #region Imports

    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using DFA.Edge;
    using DFA.State;
    using DynamicXml.Scanner.LexemeReader;
    using Lexeme;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    /// <summary>
    ///     Summary description for RegexLexemeReaderTests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DfaLexemeReaderTests
    {
        private static readonly IState ColonSymbolTerminalState = new TerminalState(LexemeType.ColonSymbol, nameof(ColonSymbolTerminalState));
        private static readonly IState DoubleQuoteSymbolTerminalState = new TerminalState(LexemeType.DoubleQuoteSymbol, nameof(DoubleQuoteSymbolTerminalState));
        private static readonly IState SingleQuoteSymbolTerminalState = new TerminalState(LexemeType.SingleQuoteSymbol, nameof(SingleQuoteSymbolTerminalState));
        private static readonly IState EqualSymbolTerminalState = new TerminalState(LexemeType.EqualSymbol, nameof(EqualSymbolTerminalState));
        private static readonly IState SlashSymbolTerminalState = new TerminalState(LexemeType.SlashSymbol, nameof(SlashSymbolTerminalState));
        private static readonly IState GreaterThanSymbolTerminalState = new TerminalState(LexemeType.GreaterThanSymbol, nameof(GreaterThanSymbolTerminalState));
        private static readonly IState LessThanSymbolTerminalState = new TerminalState(LexemeType.LessThanSymbol, nameof(LessThanSymbolTerminalState));
        private static readonly IState QuestionMarkSymbolTerminalState = new TerminalState(LexemeType.QuestionMarkSymbol, nameof(QuestionMarkSymbolTerminalState));
        private static readonly IState WhitespaceSymbolTerminalState = new TerminalState(LexemeType.WhitespaceSymbol, nameof(WhitespaceSymbolTerminalState));

        private static readonly IState WhitespaceSymbolNonterminalState = new NonterminalState(new IEdge[]
        {
            new LocalEdge(buffer => buffer != null && char.IsWhiteSpace(buffer[0])),
            new TransitionEdge(buffer => buffer == null || !char.IsWhiteSpace(buffer[0]), WhitespaceSymbolTerminalState)
        }, nameof(WhitespaceSymbolNonterminalState));

        private readonly IState _initialXmlState = new NonterminalState(new IEdge[]
        {
            new TransitionEdge(buffer => buffer[0] == ':', ColonSymbolTerminalState),
            new TransitionEdge(buffer => buffer[0] == '"', DoubleQuoteSymbolTerminalState),
            new TransitionEdge(buffer => buffer[0] == '\'', SingleQuoteSymbolTerminalState),
            new TransitionEdge(buffer => buffer[0] == '=', EqualSymbolTerminalState),
            new TransitionEdge(buffer => buffer[0] == '/', SlashSymbolTerminalState),
            new TransitionEdge(buffer => buffer[0] == '>', GreaterThanSymbolTerminalState),
            new TransitionEdge(buffer => buffer[0] == '<', LessThanSymbolTerminalState),
            new TransitionEdge(buffer => buffer[0] == '?', QuestionMarkSymbolTerminalState),
            new TransitionEdge(buffer => char.IsWhiteSpace(buffer[0]), WhitespaceSymbolNonterminalState)
        });
        

        [TestMethod]
        public void DfaTerminalLexemeReadingTest()
        {
            const string testString = ":\"'=/><?";

            var expectedLexemes = new[]
            {
                LexemeType.ColonSymbol,
                LexemeType.DoubleQuoteSymbol,
                LexemeType.SingleQuoteSymbol,
                LexemeType.EqualSymbol,
                LexemeType.SlashSymbol,
                LexemeType.GreaterThanSymbol,
                LexemeType.LessThanSymbol,
                LexemeType.QuestionMarkSymbol,
                LexemeType.Eof
            };

            var reader = new DfaLexemeReader(new MemoryStream(Encoding.UTF8.GetBytes(testString)), 1, _initialXmlState);

            foreach (var lexemeType in expectedLexemes)
            {
                var lexeme = reader.GetNextLexemeFromBuffer();
                Assert.AreEqual(lexemeType, lexeme.Type);
            }
        }

        [TestMethod]
        public void DfaWhitespaceOnlyLexemeReadingTest()
        {
            const string testString = " \t\n  ";

            var expectedLexemes = new[]
            {
                LexemeType.WhitespaceSymbol,
                LexemeType.Eof
            };

            var reader = new DfaLexemeReader(new MemoryStream(Encoding.UTF8.GetBytes(testString)), 1, _initialXmlState);

            foreach (var lexemeType in expectedLexemes)
            {
                var lexeme = reader.GetNextLexemeFromBuffer();
                Assert.AreEqual(lexemeType, lexeme.Type);
            }
        }

        //[TestMethod]
        //public void XmlPrologLexemeizingTest()
        //{
        //    const string testString = "<?xml version=\"1.0\" encoding='UTF-8'?>";

        //    var expectedLexemes = new[]
        //    {
        //        LexemeType.LessThanSymbol,
        //        LexemeType.QuestionMarkSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.WhitespaceSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.EqualSymbol,
        //        LexemeType.DoubleQuoteSymbol,
        //        LexemeType.Version,
        //        LexemeType.DoubleQuoteSymbol,
        //        LexemeType.WhitespaceSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.EqualSymbol,
        //        LexemeType.SingleQuoteSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.SingleQuoteSymbol,
        //        LexemeType.QuestionMarkSymbol,
        //        LexemeType.GreaterThanSymbol,
        //        LexemeType.Eof
        //    };

        //    var expectedIdentifiers = new[]
        //    {
        //        "xml",
        //        "version",
        //        "encoding",
        //        "UTF-8"
        //    };

        //    var expectedIdentifierIndex = 0;

        //    var reader = new RegexLexemeReader(testString);

        //    foreach (var lexemeType in expectedLexemes)
        //    {
        //        var lexeme = reader.GetNextLexemeFromBuffer();
        //        Assert.AreEqual(lexemeType, lexeme.Type);

        //        if (lexeme.Type == LexemeType.Identifier)
        //            Assert.AreEqual(expectedIdentifiers[expectedIdentifierIndex++], lexeme.Literal);
        //    }
        //}

        //[TestMethod]
        //public void TagParsingTest()
        //{
        //    const string testString =
        //        "<Parent attrib=\"New Attribute\">\n\t<Child/>\n\t<Child>Some info</Child><Child id='2'/>\n</Parent>";

        //    var expectedLexemes = new[]
        //    {
        //        LexemeType.LessThanSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.WhitespaceSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.EqualSymbol,
        //        LexemeType.DoubleQuoteSymbol,
        //        LexemeType.Data,
        //        LexemeType.DoubleQuoteSymbol,
        //        LexemeType.GreaterThanSymbol,
        //        LexemeType.WhitespaceSymbol,
        //        LexemeType.LessThanSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.SlashSymbol,
        //        LexemeType.GreaterThanSymbol,
        //        LexemeType.WhitespaceSymbol,
        //        LexemeType.LessThanSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.GreaterThanSymbol,
        //        LexemeType.Data,
        //        LexemeType.LessThanSymbol,
        //        LexemeType.SlashSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.GreaterThanSymbol,
        //        LexemeType.LessThanSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.WhitespaceSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.EqualSymbol,
        //        LexemeType.SingleQuoteSymbol,
        //        LexemeType.Data,
        //        LexemeType.SingleQuoteSymbol,
        //        LexemeType.SlashSymbol,
        //        LexemeType.GreaterThanSymbol,
        //        LexemeType.WhitespaceSymbol,
        //        LexemeType.LessThanSymbol,
        //        LexemeType.SlashSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.GreaterThanSymbol,
        //        LexemeType.Eof
        //    };

        //    var expectedLiterals = new[]
        //    {
        //        "Parent",
        //        "attrib",
        //        "New Attribute",
        //        "Child",
        //        "Child",
        //        "Some info",
        //        "Child",
        //        "Child",
        //        "id",
        //        "2",
        //        "Parent"
        //    };

        //    var expectedIdentifierIndex = 0;

        //    var reader = new RegexLexemeReader(testString);

        //    foreach (var lexemeType in expectedLexemes)
        //    {
        //        var lexeme = reader.GetNextLexemeFromBuffer(lexemeType);
        //        Assert.AreEqual(lexemeType, lexeme.Type);

        //        if (lexeme.Type == LexemeType.Identifier ||
        //            lexeme.Type == LexemeType.Data)
        //            Assert.AreEqual(expectedLiterals[expectedIdentifierIndex++], lexeme.Literal);
        //    }
        //}

        //[TestMethod]
        //public void CommentParsingTest()
        //{
        //    const string testString =
        //        "<!--Comment--><tag><!-- New Comment with <tag> and \t\n\t some whitespace --></tag>";

        //    var expectedLexemes = new[]
        //    {
        //        LexemeType.Comment,
        //        LexemeType.LessThanSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.GreaterThanSymbol,
        //        LexemeType.Comment,
        //        LexemeType.LessThanSymbol,
        //        LexemeType.SlashSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.GreaterThanSymbol
        //    };

        //    var reader = new RegexLexemeReader(testString);

        //    foreach (var lexemeType in expectedLexemes)
        //    {
        //        var lexeme = reader.GetNextLexemeFromBuffer(lexemeType);
        //        Assert.AreEqual(lexemeType, lexeme.Type);
        //    }
        //}

        //[TestMethod]
        //public void CDataParsingTest()
        //{
        //    const string testString =
        //        "<!--Comment with <![CDATA ]]> --><tag><![CDATA[New character\ndata with a <tag>]]></tag>";

        //    var expectedLexemes = new[]
        //    {
        //        LexemeType.Comment,
        //        LexemeType.LessThanSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.GreaterThanSymbol,
        //        LexemeType.CData,
        //        LexemeType.LessThanSymbol,
        //        LexemeType.SlashSymbol,
        //        LexemeType.Identifier,
        //        LexemeType.GreaterThanSymbol
        //    };

        //    var reader = new RegexLexemeReader(testString);

        //    foreach (var lexemeType in expectedLexemes)
        //    {
        //        var lexeme = reader.GetNextLexemeFromBuffer(lexemeType);
        //        Assert.AreEqual(lexemeType, lexeme.Type);
        //    }
        //}
    }
}
