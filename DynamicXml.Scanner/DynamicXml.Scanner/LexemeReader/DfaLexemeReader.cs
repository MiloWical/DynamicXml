namespace DynamicXml.Scanner.LexemeReader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using DFA.State;
    using Lexeme;

    public class DfaLexemeReader : ILexemeReader
    {
        private readonly StreamReader _reader;
        private char[] _buffer;
        private bool _endOfStream;
        private readonly IState _initialState;

        private static readonly Lexeme EofLexeme = new Lexeme(LexemeType.Eof, string.Empty);

        public DfaLexemeReader(Stream inputStream, int lookaheadBufferSize, IState initialState)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (!inputStream.CanRead)
                throw new ArgumentException("The stream could not be read from.", nameof(inputStream));

            _initialState = initialState ?? throw new ArgumentNullException(nameof(initialState));

            _reader = new StreamReader(inputStream);
            _buffer = new char[lookaheadBufferSize];
            _endOfStream = false;
        }

        public Lexeme GetNextLexemeFromBuffer(LexemeType specifiedLexeme = LexemeType.Unspecified)
        {
            if (_endOfStream)
                return EofLexeme;

            var currentState = _initialState;
            var lexeme = new List<char>();

            while (!(currentState is TerminalState))
            {
                AdvanceBuffer();

                if (_endOfStream)
                    return EofLexeme;

                currentState = currentState.TransitionToNextState(_buffer, lexeme);
            }

            var terminalStateLexeme = new Lexeme(((TerminalState)currentState).Type, new string(lexeme.ToArray()));

            return terminalStateLexeme;
        }

        private void AdvanceBuffer()
        {
            var charactersRead = _reader.Read(_buffer, 0, _buffer.Length);

            if (charactersRead > 0) return;

            _endOfStream = true;
            _reader.Close();
            _reader.Dispose();
            _buffer = null;
        }
    }
}
