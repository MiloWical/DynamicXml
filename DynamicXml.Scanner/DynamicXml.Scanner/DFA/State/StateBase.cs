namespace DynamicXml.Scanner.DFA.State
{
    public abstract class StateBase : IState
    {
        public string Name { get; }

        protected StateBase(string name)
        {
            Name = name;
        }

        public abstract IState TransitionToNextState();
    }
}
