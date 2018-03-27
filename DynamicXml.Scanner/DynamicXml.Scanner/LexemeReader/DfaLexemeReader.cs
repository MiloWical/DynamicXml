namespace DynamicXml.Scanner.LexemeReader
{
    using System;
    using System.IO;
    using DFA.State;
    using Lexeme;

    public class DfaLexemeReader : ILexemeReader
    {
        private readonly StreamReader _reader;
        private char[] _buffer;
        private int _bufferIndex;
        private bool _endOfStream;
        private readonly IState _initialState;

        private static readonly Lexeme UndefinedLexeme = new Lexeme(LexemeType.Undefined, string.Empty);

        public DfaLexemeReader(Stream inputStream, int lookaheadBufferSize, IState initialState)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (!inputStream.CanRead)
                throw new ArgumentException("The stream could not be read from.", nameof(inputStream));

            _initialState = initialState ?? throw new ArgumentNullException(nameof(initialState));

            _reader = new StreamReader(inputStream);
            _buffer = new char[lookaheadBufferSize];
            _bufferIndex = 0;
            _endOfStream = false;
        }

        public Lexeme GetNextLexemeFromBuffer(LexemeType specifiedLexeme = LexemeType.Unspecified)
        {
            if (_endOfStream)
                return UndefinedLexeme;

            var currentState = _initialState;

            while (!(currentState is TerminalState))
            {
                AdvanceBuffer();
                currentState = currentState.TransitionToNextState(_buffer);
            }

            return ((TerminalState) currentState).Lexeme;
        }

        private void AdvanceBuffer()
        {
            var charactersRead = _reader.Read(_buffer, _bufferIndex, _buffer.Length);

            if (charactersRead == 0)
            {
                _endOfStream = true;
                _reader.Close();
                _reader.Dispose();
                _buffer = null;
            }

            _bufferIndex += charactersRead;
        }
    }
}
