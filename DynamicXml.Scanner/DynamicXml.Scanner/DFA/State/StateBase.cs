namespace DynamicXml.Scanner.DFA.State
{
    using System;
    using Edge;

    public abstract class StateBase : IState
    {
        public string Name { get; }

        private IEdge[] _transitions;

        protected StateBase(IEdge[] transitions, string name = "")
        {
            _transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));
            Name = name;
        }

        public abstract IState TransitionToNextState(char[] buffer);
    }
}
