namespace DynamicXml.Scanner.DFA.Edge
{
    using System;
    using Container;
    using State;

    public class TransitionEdge : IEdge
    {
        private readonly Func<char[], bool> _transitionFunction;
        private readonly Func<IState> _edgeFunction;
        private readonly Action _bufferAdvanceAction;

        public TransitionEdge(Func<char[], bool> transitionFunction, Func<IState> edgeFunction, Action bufferAdvanceAction)
        {
            _transitionFunction = transitionFunction ?? throw new ArgumentNullException(nameof(transitionFunction));
            _edgeFunction = edgeFunction ?? throw new ArgumentNullException(nameof(edgeFunction));
            _bufferAdvanceAction = bufferAdvanceAction ?? throw new ArgumentNullException(nameof(bufferAdvanceAction));
        }

        public bool Transition(char[] buffer)
        {
            return _transitionFunction(buffer);
        }

        public void AdvanceBuffer()
        {
            _bufferAdvanceAction.Invoke();
        }

        public IState NextState => _edgeFunction.Invoke();
    }
}
