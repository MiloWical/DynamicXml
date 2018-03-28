namespace DynamicXml.Scanner.DFA.State
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using Edge;
    using Exception;

    #endregion

    public class NonterminalState : StateBase
    {
        public NonterminalState(IEdge[] transitions, string name = null) : base(transitions, name)
        {
        }

        public override IState TransitionToNextState(char[] buffer, ICollection<char> currentLexeme)
        {
            foreach (var edge in Transitions)
            {
                if (!edge.Transition(buffer)) continue;

                var nextState = GetNextStateFromEdge(edge);

                switch (nextState)
                {
                    case null:
                        continue;
                    case TerminalState _:
                        return nextState;
                }

                foreach (var c in buffer)
                    currentLexeme.Add(c);
                    
                return nextState;
            }

            throw new IllegalBufferStateException(this, buffer);
        }

        private IState GetNextStateFromEdge(IEdge edge)
        {
            if (edge is TransitionEdge transitionEdge)
                return transitionEdge.NextState;

            return this;
        }
    }
}
