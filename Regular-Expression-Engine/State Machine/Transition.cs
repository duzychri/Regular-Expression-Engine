using System.Diagnostics;

namespace Regular_Expression_Engine
{
    [DebuggerDisplay("{ToString()}"), DebuggerTypeProxy(typeof(TransitionDebugView))]
    internal struct Transition
    {
        public State State { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsAnyCharacter { get; set; }
        public char Character { get; set; }

        public override string ToString()
        {
            return IsEmpty ? $"State = '{State}', Character = 'Empty'" : $"State = '{State}', Character = '{Character}'";
        }

        internal class TransitionDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Transition[] Connections { get => transition.State.Connections.ToArray(); }

            private readonly Transition transition;

            public TransitionDebugView(Transition transition)
            {
                this.transition = transition;
            }
        }
    }
}
