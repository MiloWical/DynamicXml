namespace DynamicXml.Scanner.DFA.Edge
{
    using System;
    using State;

    public class EpsilonEdge : TransitionEdge
    {
        public EpsilonEdge(Func<char[], bool> transitionFunction, Func<IState> edgeFunction) : base(transitionFunction,
            edgeFunction)
        {
        }
    }
}
