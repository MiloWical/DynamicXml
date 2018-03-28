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
        public NonterminalState(IEdge[] transitions, string name = "") : base(transitions, name)
        {
        }

        public override IState TransitionToNextState(char[] buffer, ICollection<char> currentLexeme)
        {
            foreach (var edge in Transitions)
            {
                var nextState = edge.Transition(buffer);

                if (nextState != null)
                {
                    foreach (var c in buffer)
                        currentLexeme.Add(c);

                    return nextState;
                }
            }

            throw new IllegalBufferStateException(this, buffer);
        }
    }
}
