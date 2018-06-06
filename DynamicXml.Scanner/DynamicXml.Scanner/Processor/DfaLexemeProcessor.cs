﻿namespace DynamicXml.Scanner.Processor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DFA.BufferReader;
    using DFA.Container;
    using DFA.Edge;
    using DFA.State;
    using Lexeme;

    public class DfaLexemeProcessor : ILexemeProcessor
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

        //private static IState _versionDigitNonterminalState;

        //private static IState _versionNonterminalState;

        //private static IState _versionDotNonterminalState;

        private static IState _eofTerminalState;

        private static IState _initialXmlState;

        private readonly IStateContainer _stateContainer;

        private readonly IBufferReader _bufferReader;

        private readonly Action _bufferAdvanceAction;

        public static string[] ReservedWords = {
            "version",
            "xmlns"
        };

        public DfaLexemeProcessor(IStateContainer stateContainer, IBufferReader bufferReader)
        {
            _stateContainer = stateContainer ?? throw new ArgumentNullException(nameof(stateContainer));
            _bufferReader = bufferReader ?? throw new ArgumentNullException(nameof(bufferReader));

            _bufferAdvanceAction = bufferReader.AdvanceBuffer;

            InitializeStates();
            BuildLexemeToStateMap();
        }

        private void InitializeStates()
        {
            _colonSymbolTerminalState = new TerminalState(LexemeType.ColonSymbol, nameof(_colonSymbolTerminalState));
            _stateContainer.Register(_colonSymbolTerminalState, nameof(_colonSymbolTerminalState));

            _doubleQuoteSymbolTerminalState =
                new TerminalState(LexemeType.DoubleQuoteSymbol, nameof(_doubleQuoteSymbolTerminalState));
            _stateContainer.Register(_doubleQuoteSymbolTerminalState, nameof(_doubleQuoteSymbolTerminalState));

            _singleQuoteSymbolTerminalState =
                new TerminalState(LexemeType.SingleQuoteSymbol, nameof(_singleQuoteSymbolTerminalState));
            _stateContainer.Register(_singleQuoteSymbolTerminalState, nameof(_singleQuoteSymbolTerminalState));

            _equalSymbolTerminalState = new TerminalState(LexemeType.EqualSymbol, nameof(_equalSymbolTerminalState));
            _stateContainer.Register(_equalSymbolTerminalState, nameof(_equalSymbolTerminalState));

            _slashSymbolTerminalState = new TerminalState(LexemeType.SlashSymbol, nameof(_slashSymbolTerminalState));
            _stateContainer.Register(_slashSymbolTerminalState, nameof(_slashSymbolTerminalState));

            _greaterThanSymbolTerminalState =
                new TerminalState(LexemeType.GreaterThanSymbol, nameof(_greaterThanSymbolTerminalState));
            _stateContainer.Register(_greaterThanSymbolTerminalState, nameof(_greaterThanSymbolTerminalState));

            _lessThanSymbolTerminalState =
                new TerminalState(LexemeType.LessThanSymbol, nameof(_lessThanSymbolTerminalState));
            _stateContainer.Register(_lessThanSymbolTerminalState, nameof(_lessThanSymbolTerminalState));

            _questionMarkSymbolTerminalState =
                new TerminalState(LexemeType.QuestionMarkSymbol, nameof(_questionMarkSymbolTerminalState));
            _stateContainer.Register(_questionMarkSymbolTerminalState, nameof(_questionMarkSymbolTerminalState));

            _whitespaceSymbolTerminalState =
                new TerminalState(LexemeType.WhitespaceSymbol, nameof(_whitespaceSymbolTerminalState));
            _stateContainer.Register(_whitespaceSymbolTerminalState, nameof(_whitespaceSymbolTerminalState));

            _identifierTerminalState =
                new TerminalState(LexemeType.Identifier, nameof(_identifierTerminalState));
            _stateContainer.Register(_identifierTerminalState, nameof(_identifierTerminalState));

            _versionTerminalState =
                new TerminalState(LexemeType.Version, nameof(_versionTerminalState));
            _stateContainer.Register(_versionTerminalState, nameof(_versionTerminalState));

            _whitespaceSymbolNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer != null && char.IsWhiteSpace(buffer[0]),
                    () => _stateContainer[nameof(_whitespaceSymbolNonterminalState)],
                    _bufferAdvanceAction),
                new EpsilonEdge(buffer => buffer == null || !char.IsWhiteSpace(buffer[0]),
                    () => _stateContainer[nameof(_whitespaceSymbolTerminalState)])
            }, nameof(_whitespaceSymbolNonterminalState));
            _stateContainer.Register(_whitespaceSymbolNonterminalState, nameof(_whitespaceSymbolNonterminalState));

            _identifierCharacters = new[] { '-', '_', '.' };

            _identifierNonterminalState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => char.IsLetterOrDigit(buffer[0]) || _identifierCharacters.Contains(buffer[0]),
                    () => _stateContainer[nameof(_identifierNonterminalState)],
                    _bufferAdvanceAction),
                //TODO: Add a new epsilon edge here that does the reserved word lookup and transition to the appropriate
                //TODO: terminal state for the reserved lexeme.
                new EpsilonEdge(
                    buffer => !(char.IsLetterOrDigit(buffer[0]) || _identifierCharacters.Contains(buffer[0])),
                    () => _stateContainer[nameof(_identifierTerminalState)]) 
            }, nameof(_identifierNonterminalState));
            _stateContainer.Register(_identifierNonterminalState, nameof(_identifierNonterminalState));

            //_versionNonterminalState = new NonterminalState(new IEdge[]
            //{
            //    new LocalEdge(buffer => char.IsDigit(buffer[0])),
            //    new TransitionEdge(buffer => buffer[0] == '.', _versionDotNonterminalState), 
            //    new TransitionEdge(buffer => !char.IsDigit(buffer[0]), _versionTerminalState) 
            //}, nameof(_versionNonterminalState));
            //_stateContainer.Register(_versionNonterminalState, nameof(_versionNonterminalState));

            //_versionDotNonterminalState = new NonterminalState(new IEdge[]
            //{
            //    new TransitionEdge(buffer => char.IsDigit(buffer[0]), _versionNonterminalState)
            //}, nameof(_versionDotNonterminalState));
            //_stateContainer.Register(_versionDotNonterminalState, nameof(_versionDotNonterminalState));

            _eofTerminalState = new TerminalState(LexemeType.Eof, nameof(_eofTerminalState));
            _stateContainer.Register(_eofTerminalState, nameof(_eofTerminalState));

            _initialXmlState = new NonterminalState(new IEdge[]
            {
                new TransitionEdge(buffer => buffer[0] == ':',
                    () => _stateContainer[nameof(_colonSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '"', 
                    () => _stateContainer[nameof(_doubleQuoteSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '\'', 
                    () => _stateContainer[nameof(_singleQuoteSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '=',
                    () => _stateContainer[nameof(_equalSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '/', 
                    () => _stateContainer[nameof(_slashSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '>', 
                    () => _stateContainer[nameof(_greaterThanSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '<', 
                    () => _stateContainer[nameof(_lessThanSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => buffer[0] == '?',
                    () => _stateContainer[nameof(_questionMarkSymbolTerminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => char.IsWhiteSpace(buffer[0]), 
                    () => _stateContainer[nameof(_whitespaceSymbolNonterminalState)],
                    _bufferAdvanceAction),
                new TransitionEdge(buffer => char.IsLetterOrDigit(buffer[0]) || _identifierCharacters.Contains(buffer[0]), 
                    () => _stateContainer[nameof(_identifierNonterminalState)],
                    _bufferAdvanceAction)
                //new TransitionEdge(buffer => char.IsDigit(buffer[0]), _versionNonterminalState)
            });
        }

        private void BuildLexemeToStateMap()
        {
            //TODO: Finish the states needed to complete the map!

            //_lexemeToStateMap =
            //    new Dictionary<LexemeType, IState>
            //    {
            //        { LexemeType.Unspecified, null },
            //        { LexemeType.Undefined, null },
            //        //{ LexemeType.Comment, ??? },
            //        //{ LexemeType.CData, ??? },
            //        { LexemeType.LessThanSymbol, _lessThanSymbolTerminalState },
            //        { LexemeType.GreaterThanSymbol, _greaterThanSymbolTerminalState },
            //        { LexemeType.SlashSymbol, _slashSymbolTerminalState },
            //        { LexemeType.EqualSymbol, _equalSymbolTerminalState },
            //        { LexemeType.SingleQuoteSymbol, _singleQuoteSymbolTerminalState },
            //        { LexemeType.DoubleQuoteSymbol, _doubleQuoteSymbolTerminalState },
            //        { LexemeType.WhitespaceSymbol, _whitespaceSymbolNonterminalState },
            //        //{ LexemeType.OptionalWhitespaceSymbol, ??? },
            //        { LexemeType.ColonSymbol, _colonSymbolTerminalState },
            //        { LexemeType.QuestionMarkSymbol, _questionMarkSymbolTerminalState },
            //        { LexemeType.Version, _versionNonterminalState },
            //        { LexemeType.Identifier, _identifierNonterminalState },
            //        //{ LexemeType.Data, ??? },
            //        //{ LexemeType.Eof, ??? }
            //    };

            _stateContainer.Register(null, LexemeType.Unspecified.ToString());
            _stateContainer.Register(null, LexemeType.Undefined.ToString());
            //_stateContainer.Register(???, LexemeType.Comment.ToString());
            //_stateContainer.Register(???, LexemeType.CData.ToString());
            _stateContainer.Register(_lessThanSymbolTerminalState, LexemeType.LessThanSymbol.ToString());
            _stateContainer.Register(_greaterThanSymbolTerminalState, LexemeType.GreaterThanSymbol.ToString());
            _stateContainer.Register(_slashSymbolTerminalState, LexemeType.SlashSymbol.ToString());
            _stateContainer.Register(_equalSymbolTerminalState, LexemeType.EqualSymbol.ToString());
            _stateContainer.Register(_singleQuoteSymbolTerminalState, LexemeType.SingleQuoteSymbol.ToString());
            _stateContainer.Register(_doubleQuoteSymbolTerminalState, LexemeType.DoubleQuoteSymbol.ToString());
            _stateContainer.Register(_whitespaceSymbolNonterminalState, LexemeType.WhitespaceSymbol.ToString());
            //_stateContainer.Register(???, LexemeType.OptionalWhitespaceSymbol.ToString());
            _stateContainer.Register(_colonSymbolTerminalState, LexemeType.ColonSymbol.ToString());
            _stateContainer.Register(_questionMarkSymbolTerminalState, LexemeType.QuestionMarkSymbol.ToString());
            //_stateContainer.Register(_versionNonterminalState, LexemeType.Version.ToString());
            _stateContainer.Register(_identifierNonterminalState, LexemeType.Identifier.ToString());
            //_stateContainer.Register(???, LexemeType.Data);
            _stateContainer.Register(_eofTerminalState, LexemeType.Eof.ToString());
        }

        public IState this[LexemeType type] => GetStartState(type);

        public IState GetStartState(LexemeType type = LexemeType.Unspecified)
        {
            //return type == LexemeType.Unspecified ? _initialXmlState : _lexemeToStateMap[type];
            return type == LexemeType.Unspecified ? _initialXmlState : _stateContainer[type.ToString()];
        }

        public IState GetNextState(IState currentState, ICollection<char> currentLexeme)
        {
            return _bufferReader.EndOfStream
                ? _stateContainer[LexemeType.Eof.ToString()]
                : currentState.TransitionToNextState(_bufferReader.Buffer, currentLexeme);
        }
    }
}