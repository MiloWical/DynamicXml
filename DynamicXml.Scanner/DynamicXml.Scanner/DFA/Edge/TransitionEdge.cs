﻿namespace DynamicXml.Scanner.DFA.Edge
{
    using System;
    using State;

    public class TransitionEdge : IEdge
    {
        private readonly Func<char[], bool> _transitionFunction;

        public TransitionEdge(Func<char[], bool> transitionFunction, IState nextState)
        {
            _transitionFunction = transitionFunction ?? throw new ArgumentNullException(nameof(transitionFunction));
            NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
        }

        public bool Transition(char[] buffer)
        {
            return _transitionFunction(buffer);
        }

        public IState NextState { get; }
    }
}
