namespace DynamicXml.Scanner.DFA.State
{
    using System;
    using System.Collections.Generic;
    using Edge;

    public abstract class StateBase : IState
    {
        public string Name { get; }

        protected IEdge[] Transitions;

        protected StateBase(IEdge[] transitions, string name = "")
        {
            Transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));
            Name = name;
        }

        public abstract IState TransitionToNextState(char[] buffer, ICollection<char> currentLexeme);
    }
}
