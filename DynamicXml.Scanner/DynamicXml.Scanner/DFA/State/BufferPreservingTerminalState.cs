namespace DynamicXml.Scanner.DFA.State
{
    #region Imports

    using System.Collections.Generic;
    using Lexeme;

    #endregion

    public class BufferPreservingTerminalState : TerminalState, IBufferPreservingState
    {
        public BufferPreservingTerminalState(LexemeType lexemeType, string name = null) : base (lexemeType, name)
        { }
    }
}
