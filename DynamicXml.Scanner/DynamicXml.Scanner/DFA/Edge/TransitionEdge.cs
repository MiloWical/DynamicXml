namespace DynamicXml.Scanner.DFA.Edge
{
    using System;
    using State;

    public class TransitionEdge : IEdge
    {
        private readonly Func<char[], bool> _transitionFunction;

        private readonly IState _finalState;

        public TransitionEdge(Func<char[], bool> transitionFunction, IState finalState)
        {
            _transitionFunction = transitionFunction ?? throw new ArgumentNullException(nameof(transitionFunction));
            _finalState = finalState ?? throw new ArgumentNullException(nameof(finalState));
        }

        public IState Transition(char[] buffer)
        {
            return _transitionFunction(buffer) ? _finalState : null;
        }
    }
}
