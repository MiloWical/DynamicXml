namespace DynamicXml.Scanner.DFA.Container
{
    using System;
    using Edge;
    using Lexeme;
    using State;
    using System.Linq;
    using BufferReader;

    public sealed class DefaultDfaStateContainer : DfaStateContainer
    {
        private static Action _bufferAdvanceAction;

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

        private static IState _eofTerminalState;

        private static IState _initialXmlState;

        public DefaultDfaStateContainer(Action bufferAdvanceAction)
        {
            _bufferAdvanceAction = bufferAdvanceAction ?? throw new ArgumentNullException(nameof(bufferAdvanceAction));

            InitializeStates();
            BuildLexemeToStateMap();
        }

        private void InitializeStates()
        {
            _colonSymbolTerminalState = new TerminalState(LexemeType.ColonSymbol, nameof(_colonSymbolTerminalState));
            Register(_colonSymbolTerminalState, nameof(_colonSymbolTerminalState));

            _doubleQuoteSymbolTerminalState =
                new TerminalState(LexemeType.DoubleQuoteSymbol, nameof(_doubleQuoteSymbolTerminalState));
            Register(_doubleQuoteSymbolTerminalState, nameof(_doubleQuoteSymbolTerminalState));

            _singleQuoteSymbolTerminalState =
                new TerminalState(LexemeType.SingleQuoteSymbol, nameof(_singleQuoteSymbolTerminalState));
            Register(_singleQuoteSymbolTerminalState, nameof(_singleQuoteSymbolTerminalState));

            _equalSymbolTerminalState = new TerminalState(LexemeType.EqualSymbol, nameof(_equalSymbolTerminalState));
            Register(_equalSymbolTerminalState, nameof(_equalSymbolTerminalState));

            _slashSymbolTerminalState = new TerminalState(LexemeType.SlashSymbol, nameof(_slashSymbolTerminalState));
            Register(_slashSymbolTerminalState, nameof(_slashSymbolTerminalState));

            _greaterThanSymbolTerminalState =
                new TerminalState(LexemeType.GreaterThanSymbol, nameof(_greaterThanSymbolTerminalState));
            Register(_greaterThanSymbolTerminalState, nameof(_greaterThanSymbolTerminalState));

            _lessThanSymbolTerminalState =
                new TerminalState(LexemeType.LessThanSymbol, nameof(_lessThanSymbolTerminalState));
            Register(_lessThanSymbolTerminalState, nameof(_lessThanSymbolTerminalState));

            _questionMarkSymbolTerminalState =
                new TerminalState(LexemeType.QuestionMarkSymbol, nameof(_questionMarkSymbolTerminalState));
            Register(_questionMarkSymbolTerminalState, nameof(_questionMarkSymbolTerminalState));

            _whitespaceSymbolTerminalState =
                new TerminalState(LexemeType.WhitespaceSymbol, nameof(_whitespaceSymbolTerminalState));
            Register(_whitespaceSymbolTerminalState, nameof(_whitespaceSymbolTerminalState));

            _identifierTerminalState =
                new TerminalState(LexemeType.Identifier, nameof(_identifierTerminalState));
            Register(_identifierTerminalState, nameof(_identifierTerminalState));

            _versionTerminalState =
                new TerminalState(LexemeType.Version, nameof(_versionTerminalState));
            Register(_versionTerminalState, nameof(_versionTerminalState));

            _whitespaceSymbolNonterminalState = new NonterminalState(new IEdge[]
            {
                new EpsilonEdge(buffer => buffer == null || !char.IsWhiteSpace(buffer[0]),
                    () => this[nameof(_whitespaceSymbolTerminalState)]),
                new TransitionEdge(buffer => buffer != null && char.IsWhiteSpace(buffer[0]),
                    () => this[nameof(_whitespaceSymbolNonterminalState)], 
                    _bufferAdvanceAction)
            }, nameof(_whitespaceSymbolNonterminalState));
            Register(_whitespaceSymbolNonterminalState, nameof(_whitespaceSymbolNonterminalState));

            _identifierCharacters = new[] { '-', '_', '.' };

            _identifierNonterminalState = new NonterminalState(new IEdge[]
            {
                new EpsilonEdge(buffer => buffer == null 
                                          || !(char.IsLetterOrDigit(buffer[0]) 
                                          || _identifierCharacters.Contains(buffer[0])),
                    () => this[nameof(_identifierTerminalState)]),
                new TransitionEdge(buffer => char.IsLetterOrDigit(buffer[0]) || _identifierCharacters.Contains(buffer[0]),
                    () => this[nameof(_identifierNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_identifierNonterminalState));
            Register(_identifierNonterminalState, nameof(_identifierNonterminalState));

            _versionDigitNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => char.IsDigit(buffer[0]), 
                    () => this[nameof(_versionDigitNonterminalState)],
                    _bufferAdvanceAction), 
                new TransitionEdge(buffer => buffer[0] == '.', 
                    () => this[nameof(_versionDotNonterminalState)],
                    _bufferAdvanceAction),
                new EpsilonEdge(buffer => !char.IsDigit(buffer[0]), 
                    () => this[nameof(_versionTerminalState)])
            }, nameof(_versionDigitNonterminalState));
            Register(_versionDigitNonterminalState, nameof(_versionDigitNonterminalState));

            _versionDotNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => char.IsDigit(buffer[0]), 
                    () => this[nameof(_versionDigitNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_versionDotNonterminalState));
            Register(_versionDotNonterminalState, nameof(_versionDotNonterminalState));

            _eofTerminalState = new TerminalState(LexemeType.Eof, nameof(_eofTerminalState));
            Register(_eofTerminalState, nameof(_eofTerminalState));

            _initialXmlState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == ':',
                    () => this[nameof(_colonSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '"',
                    () => this[nameof(_doubleQuoteSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '\'',
                    () => this[nameof(_singleQuoteSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '=',
                    () => this[nameof(_equalSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '/',
                    () => this[nameof(_slashSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '>',
                    () => this[nameof(_greaterThanSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '<',
                    () => this[nameof(_lessThanSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '?',
                    () => this[nameof(_questionMarkSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => char.IsWhiteSpace(buffer[0]),
                    () => this[nameof(_whitespaceSymbolNonterminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => char.IsDigit(buffer[0]),
                    () => this[nameof(_versionDigitNonterminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => char.IsLetterOrDigit(buffer[0]) || _identifierCharacters.Contains(buffer[0]),
                    () => this[nameof(_identifierNonterminalState)],
                    _bufferAdvanceAction),
                });
            Register(_initialXmlState, nameof(_initialXmlState));
        }

        private void BuildLexemeToStateMap()
        {
            //TODO: Finish the states needed to complete the map!

            Register(_initialXmlState, LexemeType.Unspecified.ToString());
            Register(null, LexemeType.Undefined.ToString());
            //Register(???, LexemeType.Comment.ToString());
            //Register(???, LexemeType.CData.ToString());
            Register(_lessThanSymbolTerminalState, LexemeType.LessThanSymbol.ToString());
            Register(_greaterThanSymbolTerminalState, LexemeType.GreaterThanSymbol.ToString());
            Register(_slashSymbolTerminalState, LexemeType.SlashSymbol.ToString());
            Register(_equalSymbolTerminalState, LexemeType.EqualSymbol.ToString());
            Register(_singleQuoteSymbolTerminalState, LexemeType.SingleQuoteSymbol.ToString());
            Register(_doubleQuoteSymbolTerminalState, LexemeType.DoubleQuoteSymbol.ToString());
            Register(_whitespaceSymbolNonterminalState, LexemeType.WhitespaceSymbol.ToString());
            //Register(???, LexemeType.OptionalWhitespaceSymbol.ToString());
            Register(_colonSymbolTerminalState, LexemeType.ColonSymbol.ToString());
            Register(_questionMarkSymbolTerminalState, LexemeType.QuestionMarkSymbol.ToString());
            Register(_versionDigitNonterminalState, LexemeType.Version.ToString());
            Register(_identifierNonterminalState, LexemeType.Identifier.ToString());
            //Register(???, LexemeType.Data);
            Register(_eofTerminalState, LexemeType.Eof.ToString());
        }
    }
}
