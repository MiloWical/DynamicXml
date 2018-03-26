namespace DynamicXml.Scanner.Exception
{
    using DFA.State;
    using Exception = System.Exception;

    public class IllegalBufferStateException : Exception
    {
        private readonly IState _illegalState;

        private readonly char[] _buffer;

        public IllegalBufferStateException(IState illegalState, char[] buffer)
        {
            _illegalState = illegalState;
            _buffer = buffer;
        }

        public override string Message => $"Illegal State '{_illegalState.GetType().FullName}' did not process correctly due to improper buffer contents: '{new string(_buffer)}'\n\n{base.Message}";
    }
}
