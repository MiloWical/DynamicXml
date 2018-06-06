namespace DynamicXml.Scanner.LexemeReader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using DFA.State;
    using Lexeme;
    using Processor;

    public class DfaLexemeReader : ILexemeReader
    {
        //private readonly StreamReader _reader;
        //private char[] _buffer;
        //private bool _endOfStream;
        private readonly ILexemeProcessor _lexemeProcessor;

        //private static readonly Lexeme EofLexeme = new Lexeme(LexemeType.Eof, string.Empty);

        //public DfaLexemeReader(Stream inputStream, int lookaheadBufferSize, ILexemeLookup lexemeLookup)
        //{
        //    if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
        //    if (!inputStream.CanRead)
        //        throw new ArgumentException("The stream could not be read from.", nameof(inputStream));

        //    _reader = new StreamReader(inputStream);
        //    _buffer = new char[lookaheadBufferSize];
        //    _endOfStream = false;
        //    _lexemeLookup = lexemeLookup ?? throw new ArgumentNullException(nameof(lexemeLookup));

        //    AdvanceBuffer(); //Preload the buffer
        //}

        public DfaLexemeReader(ILexemeProcessor lexemeProcessor)
        {
            _lexemeProcessor = lexemeProcessor ?? throw new ArgumentNullException(nameof(lexemeProcessor));

            //AdvanceBuffer(); //Preload the buffer
        }

        public Lexeme GetNextLexemeFromBuffer(LexemeType specifiedLexeme = LexemeType.Unspecified)
        {
            //if (_endOfStream)
            //    return EofLexeme;
          
            var lexeme = new List<char>();

            //var currentState = _lexemeProcessor[specifiedLexeme];
            //if(currentState is TerminalState)
            //    AdvanceBuffer();

            //while (!(currentState is TerminalState))
            //{
            //    //TODO: Change this to get the transition function, then move it to the next state.
            //    //TODO: Trigger the change for the buffer advancing to whether the function was an
            //    //TODO: epsilon edge, not the state type.
            //    currentState = currentState.TransitionToNextState(_buffer, lexeme);

            //    //if(!(currentState is IBufferPreservingState))
            //    //    AdvanceBuffer(); //Always leave the buffer one read ahead if we're not done
            //}

            IState currentState;

            for (currentState = _lexemeProcessor[specifiedLexeme];
                !(currentState is TerminalState);
                currentState = _lexemeProcessor.GetNextState(currentState, lexeme))
            {}

            var terminalStateLexeme = new Lexeme(((TerminalState)currentState).Type, new string(lexeme.ToArray()));

            return terminalStateLexeme;
        }

        //private void AdvanceBuffer()
        //{
        //    if (_endOfStream) return;

        //    var charactersRead = _reader.Read(_buffer, 0, _buffer.Length);

        //    if (charactersRead > 0) return;

        //    _endOfStream = true;
        //    _reader.Close();
        //    _reader.Dispose();
        //    _buffer = null;
        //}
    }
}
