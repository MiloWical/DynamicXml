namespace DynamicXml.Scanner.DFA.State
{
    using System;
    using System.Collections.Generic;
    using Edge;

    public abstract class StateBase : IState
    {
        public string Name { get; }

        protected ICollection<IEdge> Transitions;

        protected StateBase(ICollection<IEdge> transitions, string name = null)
        {
            Transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));
            Name = name;
        }

        public abstract IState TransitionToNextState(char[] buffer, ICollection<char> currentLexeme);
        public void AddEdge(IEdge newEdge)
        {
            if(newEdge != null)
                Transitions.Add(newEdge);
        }
    }
}
