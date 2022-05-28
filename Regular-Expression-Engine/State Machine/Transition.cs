using System;
using System.Diagnostics;

namespace Regular_Expression_Engine
{
    [DebuggerDisplay("{ToString()}"), DebuggerTypeProxy(typeof(TransitionDebugView))]
    internal struct Transition : IComparable<Transition>
    {
        public State State { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsAnyCharacter { get; set; }
        public char Character { get; set; }

        public int CompareTo(Transition other)
        {
            if (State.IsEndState && other.State.IsEndState)
            { return 0; }

            if (State.IsEndState)
            { return -1; }

            return 1;
        }

        public override string ToString()
        {
            return IsEmpty ? $"State = '{State}', Character = 'Empty'" : $"State = '{State}', Character = '{Character}'";
        }

        #region Debugging

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

        #endregion Debugging
    }
}
