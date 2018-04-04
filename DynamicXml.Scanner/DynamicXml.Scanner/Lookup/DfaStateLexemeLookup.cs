using System.Collections.Generic;

namespace DynamicXml.Scanner.DFA
{
    using Edge;
    using Lexeme;
    using Lookup;
    using State;
    using System.Linq;

    public class DfaStateLexemeLookup : ILexemeLookup
    {
        private static IState _colonSymbolTerminalState;
        private static IState _doubleQuoteSymbolTerminalState;
        private static IState _singleQuoteSymbolTerminalState;
        private static IState _equalSymbolTerminalState;
        private static IState _slashSymbolTerminalState;
        private static IState _greaterThanSymbolTerminalState;
        private static IState _lessThanSymbolTerminalState;
        private static IState _questionMarkSymbolTerminalState;

        private static IState _whitespaceSymbolTerminalState;
        private static IState _identifierTerminalState;
        private static IState _versionTerminalState;

        private static IState _whitespaceSymbolNonterminalState;

        private static char[] _identifierCharacters = { '-', '_', '.' };

        private static IState _identifierNonterminalState;

        private static IState _versionDigitNonterminalState;

        private static IState _versionDotNonterminalState;

        private static IState _initialXmlState;

        private static Dictionary<LexemeType, IState> _lexemeToStateMap;

        public DfaStateLexemeLookup()
        {
            InitializeStates();
            BuildLexemeToStateMap();
        }

        private void InitializeStates()
        {
            _colonSymbolTerminalState = new TerminalState(LexemeType.ColonSymbol, nameof(_colonSymbolTerminalState));
            _doubleQuoteSymbolTerminalState =
                new TerminalState(LexemeType.DoubleQuoteSymbol, nameof(_doubleQuoteSymbolTerminalState));
            _singleQuoteSymbolTerminalState =
                new TerminalState(LexemeType.SingleQuoteSymbol, nameof(_singleQuoteSymbolTerminalState));
            _equalSymbolTerminalState = new TerminalState(LexemeType.EqualSymbol, nameof(_equalSymbolTerminalState));
            _slashSymbolTerminalState = new TerminalState(LexemeType.SlashSymbol, nameof(_slashSymbolTerminalState));
            _greaterThanSymbolTerminalState =
                new TerminalState(LexemeType.GreaterThanSymbol, nameof(_greaterThanSymbolTerminalState));
            _lessThanSymbolTerminalState =
                new TerminalState(LexemeType.LessThanSymbol, nameof(_lessThanSymbolTerminalState));
            _questionMarkSymbolTerminalState =
                new TerminalState(LexemeType.QuestionMarkSymbol, nameof(_questionMarkSymbolTerminalState));

            _whitespaceSymbolTerminalState =
                new BufferPreservingTerminalState(LexemeType.WhitespaceSymbol, nameof(_whitespaceSymbolTerminalState));
            _identifierTerminalState =
                new BufferPreservingTerminalState(LexemeType.Identifier, nameof(_identifierTerminalState));
            _versionTerminalState =
                new BufferPreservingTerminalState(LexemeType.Version, nameof(_versionDigitNonterminalState));

            _whitespaceSymbolNonterminalState = new NonterminalState(new IEdge[]
            {
                new LocalEdge(buffer => buffer != null && char.IsWhiteSpace(buffer[0])),
                new TransitionEdge(buffer => buffer == null || !char.IsWhiteSpace(buffer[0]),
                    _whitespaceSymbolTerminalState)
            }, nameof(_whitespaceSymbolNonterminalState));

            _identifierCharacters = new[] { '-', '_', '.' };

            _identifierNonterminalState = new NonterminalState(new IEdge[]
            {
                new LocalEdge(buffer => char.IsLetterOrDigit(buffer[0]) || _identifierCharacters.Contains(buffer[0])),
                new TransitionEdge(
                    buffer => !(char.IsLetterOrDigit(buffer[0]) || _identifierCharacters.Contains(buffer[0])),
                    _identifierTerminalState)
            }, nameof(_identifierNonterminalState));

            //_versionDigitNonterminalState = new NonterminalState(new IEdge[]
            //{
            //    new LocalEdge(buffer => char.IsDigit(buffer[0])),
            //    new TransitionEdge(buffer => buffer[0] == '.', _versionDotNonterminalState),
            //    new TransitionEdge(buffer => !char.IsDigit(buffer[0]) && buffer[0] != '.', _versionTerminalState)
            //}, nameof(_versionDigitNonterminalState));

            //_versionDotNonterminalState = new NonterminalState(new IEdge[]
            //{
            //    new TransitionEdge(buffer => char.IsDigit(buffer[0]), _versionDigitNonterminalState)
            //});

            _initialXmlState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == ':', _colonSymbolTerminalState),
                new TransitionEdge(buffer => buffer[0] == '"', _doubleQuoteSymbolTerminalState),
                new TransitionEdge(buffer => buffer[0] == '\'', _singleQuoteSymbolTerminalState),

                new TransitionEdge(buffer => buffer[0] == '=', _equalSymbolTerminalState),
                new TransitionEdge(buffer => buffer[0] == '/', _slashSymbolTerminalState),
                new TransitionEdge(buffer => buffer[0] == '>', _greaterThanSymbolTerminalState),
                new TransitionEdge(buffer => buffer[0] == '<', _lessThanSymbolTerminalState),
                new TransitionEdge(buffer => buffer[0] == '?', _questionMarkSymbolTerminalState),
                new TransitionEdge(buffer => char.IsWhiteSpace(buffer[0]), _whitespaceSymbolNonterminalState),
                new TransitionEdge(buffer => char.IsLetterOrDigit(buffer[0]) || _identifierCharacters.Contains(buffer[0]), _identifierNonterminalState),
                //new TransitionEdge(buffer => char.IsDigit(buffer[0]), _versionDigitNonterminalState)
            });
        }

        private static void BuildLexemeToStateMap()
        {
            //TODO: Finish the states needed to complete the map!

            _lexemeToStateMap =
                new Dictionary<LexemeType, IState>
                {
                    { LexemeType.Unspecified, null },
                    { LexemeType.Undefined, null },
                    //{ LexemeType.Comment, ??? },
                    //{ LexemeType.CData, ??? },
                    { LexemeType.LessThanSymbol, _lessThanSymbolTerminalState },
                    { LexemeType.GreaterThanSymbol, _greaterThanSymbolTerminalState },
                    { LexemeType.SlashSymbol, _slashSymbolTerminalState },
                    { LexemeType.EqualSymbol, _equalSymbolTerminalState },
                    { LexemeType.SingleQuoteSymbol, _singleQuoteSymbolTerminalState },
                    { LexemeType.DoubleQuoteSymbol, _doubleQuoteSymbolTerminalState },
                    { LexemeType.WhitespaceSymbol, _whitespaceSymbolNonterminalState },
                    //{ LexemeType.OptionalWhitespaceSymbol, ??? },
                    { LexemeType.ColonSymbol, _colonSymbolTerminalState },
                    { LexemeType.QuestionMarkSymbol, _questionMarkSymbolTerminalState },
                    //{ LexemeType.Version, _versionDigitNonterminalState },
                    { LexemeType.Identifier, _identifierNonterminalState },
                    //{ LexemeType.Data, ??? },
                    //{ LexemeType.Eof, ??? }
                };
        }

        public IState this[LexemeType type] => GetStartState(type);

        public static IState GetStartState(LexemeType type = LexemeType.Unspecified)
        {
            return type == LexemeType.Unspecified ? _initialXmlState : _lexemeToStateMap[type];
        }
    }
}
