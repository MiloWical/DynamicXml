namespace DynamicXml.Scanner.DFA.Edge
{
    using System;
    using Container;
    using State;

    public class TransitionEdge : IEdge
    {
        private readonly Func<char[], bool> _transitionFunction;
        private readonly Func<IState> _edgeFunction;

        public TransitionEdge(Func<char[], bool> transitionFunction, Func<IState> edgeFunction)
        {
            _transitionFunction = transitionFunction ?? throw new ArgumentNullException(nameof(transitionFunction));
            _edgeFunction = edgeFunction ?? throw new ArgumentNullException(nameof(edgeFunction));
        }

        public bool Transition(char[] buffer)
        {
            return _transitionFunction(buffer);
        }

        public IState NextState => _edgeFunction.Invoke();
    }
}
