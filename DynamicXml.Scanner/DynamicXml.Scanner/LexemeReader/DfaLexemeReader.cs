namespace DynamicXml.Scanner.LexemeReader
{
    using DFA.BufferReader;
    using DFA.Container;
    using DFA.State;
    using Lexeme;
    using System;
    using System.Collections.Generic;

    public class DfaLexemeReader : ILexemeReader
    {
        private readonly IStateContainer _stateContainer;
        private readonly IBufferReader _bufferReader;

        private static readonly Lexeme EofLexeme = new Lexeme(LexemeType.Eof, string.Empty);

        public DfaLexemeReader(IStateContainer stateContainer, IBufferReader bufferReader)
        {
            _stateContainer = stateContainer ?? throw new ArgumentNullException(nameof(bufferReader));
            _bufferReader = bufferReader ?? throw new ArgumentNullException(nameof(bufferReader));

            _bufferReader.AdvanceBuffer(); //Preload the buffer
        }

        public Lexeme GetNextLexemeFromBuffer(LexemeType specifiedLexeme = LexemeType.Unspecified)
        {
            if (_bufferReader.EndOfStream)
                return EofLexeme;

            var lexeme = new List<char>();

            var currentState = _stateContainer[specifiedLexeme.ToString()];

            if (currentState is TerminalState)
                _bufferReader.AdvanceBuffer();

            while (!(currentState is TerminalState))
                currentState = currentState.TransitionToNextState(_bufferReader.Buffer, lexeme);

            var terminalStateLexeme = new Lexeme(((TerminalState)currentState).Type, new string(lexeme.ToArray()));

            return terminalStateLexeme;
        }
    }
}
