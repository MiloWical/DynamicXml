namespace DynamicXml.Scanner.DFA.State
{
    using Edge;

    public class BufferPreservingNonterminalState : NonterminalState, IBufferPreservingState

    {
        public BufferPreservingNonterminalState(IEdge[] transitions, string name = null) : base(transitions, name)
        {
        }
    }
}
