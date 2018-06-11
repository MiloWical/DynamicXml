namespace DynamicXml.Scanner.DFA.Container
{
    using System;
    using Edge;
    using Lexeme;
    using State;
    using System.Linq;

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

        private static IState _lessThanSymbolNonterminalState;
        private static IState _lessThanSymbolNonterminalStatePrime;
        private static IState _commentTerminalState;
        private static IState _whitespaceSymbolTerminalState;
        private static IState _identifierTerminalState;
        private static IState _versionTerminalState;
        private static IState _dataTerminalState;
        private static IState _cDataTerminalState;

        private static IState _commentStartNonterminalState;
        private static IState _commentBodyNonterminalState;
        private static IState _commentEndNonterminalState;
        private static IState _commentEndNonterminalStatePrime;

        private static IState _cDataStartNonterminalState;
        private static IState _cDataStartNonterminalStatePrime1;
        private static IState _cDataStartNonterminalStatePrime2;
        private static IState _cDataStartNonterminalStatePrime3;
        private static IState _cDataStartNonterminalStatePrime4;
        private static IState _cDataStartNonterminalStatePrime5;
        private static IState _cDataBodyNonterminalState;
        private static IState _cDataEndNonterminalState;
        private static IState _cDataEndNonterminalStatePrime;

        private static IState _characterDataNonterminalState;

        private static IState _whitespaceSymbolNonterminalState;

        private static IState _versionDigitNonterminalState;
        private static IState _versionDotNonterminalState;

        private static readonly char[] IdentifierCharacters = { '-', '_', '.' };
        private static IState _identifierNonterminalState;

        private static readonly char[] DataCharacters = { '&', ';', '-', '_', '\\', '/', '.', '#', '>', '?' };
        private static IState _dataNonterminalState;

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

            _dataTerminalState = 
                new TerminalState(LexemeType.Data, nameof(_dataTerminalState));
            Register(_dataTerminalState, nameof(_dataTerminalState));

            _commentTerminalState = new TerminalState(LexemeType.Comment, nameof(_commentTerminalState));
            Register(_commentTerminalState, nameof(_commentTerminalState));

            _whitespaceSymbolNonterminalState = new NonterminalState(new IEdge[]
            {
                new EpsilonEdge(buffer => buffer == null || !char.IsWhiteSpace(buffer[0]),
                    () => this[nameof(_whitespaceSymbolTerminalState)]),
                new TransitionEdge(buffer => buffer != null && char.IsWhiteSpace(buffer[0]),
                    () => this[nameof(_whitespaceSymbolNonterminalState)], 
                    _bufferAdvanceAction)
            }, nameof(_whitespaceSymbolNonterminalState));
            Register(_whitespaceSymbolNonterminalState, nameof(_whitespaceSymbolNonterminalState));

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

            _identifierNonterminalState = new NonterminalState(new IEdge[]
            {
                new EpsilonEdge(buffer => buffer == null
                                          || !IsIdentifier(buffer[0]),
                    () => this[nameof(_identifierTerminalState)]),
                new TransitionEdge(buffer => IsIdentifier(buffer[0]),
                    () => this[nameof(_identifierNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_identifierNonterminalState));
            Register(_identifierNonterminalState, nameof(_identifierNonterminalState));

            _dataNonterminalState = new NonterminalState(new IEdge[]
            {
                new EpsilonEdge(buffer => buffer == null
                                            || !IsData(buffer[0]),
                    () => this[nameof(_dataTerminalState)]),
                new TransitionEdge(buffer => IsData(buffer[0]),
                    () => this[nameof(_dataNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_dataNonterminalState));
            Register(_dataNonterminalState, nameof(_dataNonterminalState));

            _lessThanSymbolNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == '<',
                    () => this[nameof(_lessThanSymbolNonterminalStatePrime)],
                    _bufferAdvanceAction)
            }, nameof(_lessThanSymbolNonterminalState));
            Register(_lessThanSymbolNonterminalState, nameof(_lessThanSymbolNonterminalState));

            _lessThanSymbolNonterminalStatePrime = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == '!',
                    () => this[nameof(_characterDataNonterminalState)],
                    _bufferAdvanceAction),
                new EpsilonEdge(buffer => true, //We've already seen a '<' symbol - no need to reprocess
                    () => this[nameof(_lessThanSymbolTerminalState)]) 
            }, nameof(_lessThanSymbolNonterminalStatePrime));
            Register(_lessThanSymbolNonterminalStatePrime, nameof(_lessThanSymbolNonterminalStatePrime));

            _characterDataNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == '-',
                    () => this[nameof(_commentStartNonterminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '[',
                    () => this[nameof(_cDataStartNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_characterDataNonterminalState));
            Register(_characterDataNonterminalState, nameof(_characterDataNonterminalState));

            _commentStartNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == '-',
                    () => this[nameof(_commentBodyNonterminalState)],
                    _bufferAdvanceAction) 
            }, nameof(_commentStartNonterminalState));
            Register(_commentStartNonterminalState, nameof(_commentStartNonterminalState));

            _commentBodyNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == '-',
                    () => this[nameof(_commentEndNonterminalState)],
                    _bufferAdvanceAction), 
                new TransitionEdge(buffer => buffer != null,
                    () => this[nameof(_commentBodyNonterminalState)],
                    _bufferAdvanceAction) 
            }, nameof(_commentBodyNonterminalState));
            Register(_commentBodyNonterminalState, nameof(_commentBodyNonterminalState));

            _commentEndNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == '-',
                    () => this[nameof(_commentEndNonterminalStatePrime)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] != '-',
                    () => this[nameof(_commentBodyNonterminalState)],
                    _bufferAdvanceAction) 
            }, nameof(_commentEndNonterminalState));
            Register(_commentEndNonterminalState, nameof(_commentEndNonterminalState));

            _commentEndNonterminalStatePrime = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == '>',
                    () => this[nameof(_commentTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] != '>',
                    () => this[nameof(_commentBodyNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_commentEndNonterminalStatePrime));
            Register(_commentEndNonterminalStatePrime, nameof(_commentEndNonterminalStatePrime));

            _cDataStartNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == 'C',
                    () => this[nameof(_cDataStartNonterminalStatePrime1)],
                    _bufferAdvanceAction) 
            }, nameof(_cDataStartNonterminalState));
            Register(_cDataStartNonterminalState, nameof(_cDataStartNonterminalState));

            _cDataStartNonterminalStatePrime1 = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == 'D',
                    () => this[nameof(_cDataStartNonterminalStatePrime2)],
                    _bufferAdvanceAction)
            }, nameof(_cDataStartNonterminalStatePrime1));
            Register(_cDataStartNonterminalStatePrime1, nameof(_cDataStartNonterminalStatePrime1));

            _cDataStartNonterminalStatePrime2 = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == 'A',
                    () => this[nameof(_cDataStartNonterminalStatePrime3)],
                    _bufferAdvanceAction)
            }, nameof(_cDataStartNonterminalStatePrime2));
            Register(_cDataStartNonterminalStatePrime2, nameof(_cDataStartNonterminalStatePrime2));

            _cDataStartNonterminalStatePrime3 = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == 'T',
                    () => this[nameof(_cDataStartNonterminalStatePrime4)],
                    _bufferAdvanceAction)
            }, nameof(_cDataStartNonterminalStatePrime3));
            Register(_cDataStartNonterminalStatePrime3, nameof(_cDataStartNonterminalStatePrime3));

            _cDataStartNonterminalStatePrime4 = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == 'A',
                    () => this[nameof(_cDataStartNonterminalStatePrime5)],
                    _bufferAdvanceAction)
            }, nameof(_cDataStartNonterminalStatePrime4));
            Register(_cDataStartNonterminalStatePrime4, nameof(_cDataStartNonterminalStatePrime4));

            _cDataStartNonterminalStatePrime5 = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == '[',
                    () => this[nameof(_cDataBodyNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_cDataStartNonterminalStatePrime5));
            Register(_cDataStartNonterminalStatePrime5, nameof(_cDataStartNonterminalStatePrime5));

            _cDataBodyNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer != null && buffer[0] == ']',
                    () => this[nameof(_cDataEndNonterminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer != null,
                    () => this[nameof(_cDataBodyNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_cDataBodyNonterminalState));
            Register(_cDataBodyNonterminalState, nameof(_cDataBodyNonterminalState));

            _cDataEndNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer != null && buffer[0] == ']',
                    () => this[nameof(_cDataEndNonterminalStatePrime)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer != null,
                    () => this[nameof(_cDataBodyNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_cDataEndNonterminalState));
            Register(_cDataEndNonterminalState, nameof(_cDataEndNonterminalState));

            _cDataEndNonterminalStatePrime = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer != null && buffer[0] == '>',
                    () => this[nameof(_cDataTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer != null,
                    () => this[nameof(_cDataBodyNonterminalState)],
                    _bufferAdvanceAction)
            }, nameof(_cDataEndNonterminalStatePrime));
            Register(_cDataEndNonterminalStatePrime, nameof(_cDataEndNonterminalStatePrime));

            _cDataTerminalState = new TerminalState(LexemeType.CData, nameof(_cDataTerminalState));
            Register(_cDataTerminalState, nameof(_cDataTerminalState));

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
                new TransitionEdge(buffer => char.IsLetterOrDigit(buffer[0]) || IdentifierCharacters.Contains(buffer[0]),
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
            Register(_lessThanSymbolNonterminalState, LexemeType.Comment.ToString());
            Register(_lessThanSymbolNonterminalState, LexemeType.CData.ToString());
            Register(_lessThanSymbolNonterminalState, LexemeType.LessThanSymbol.ToString());
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
            Register(_dataNonterminalState, LexemeType.Data.ToString());
            Register(_eofTerminalState, LexemeType.Eof.ToString());
        }

        private static bool IsIdentifier(char bufferData)
        {
            return char.IsLetterOrDigit(bufferData) || IdentifierCharacters.Contains(bufferData);
        }

        private static bool IsData(char bufferData)
        {
            return char.IsLetterOrDigit(bufferData) || char.IsWhiteSpace(bufferData) ||
                   DataCharacters.Contains(bufferData);
        }
    }
}
