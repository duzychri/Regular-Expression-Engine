using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Regular_Expression_Engine
{
    [DebuggerDisplay("Name = {Name}, Count = {Connections.Count}"), DebuggerTypeProxy(typeof(StateDebugView))]
    internal class State
    {
        public string Name { get; }
        public List<Transition> Connections { get; } = new List<Transition>();

        public bool IsEndState => Connections.Any() == false;

        public State(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public void AddConnection(State state, char character)
        {
            Transition transition = new Transition { IsEmpty = false, IsAnyCharacter = false, Character = character, State = state };
            Connections.Add(transition);
        }

        public void AddAnyConnection(State state)
        {
            Transition transition = new Transition { IsEmpty = false, IsAnyCharacter = true, Character = '\0', State = state };
            Connections.Add(transition);
        }

        public void AddEmptyConnection(State state)
        {
            Transition transition = new Transition { IsEmpty = true, IsAnyCharacter = false, Character = '\0', State = state };
            Connections.Add(transition);
        }

        #region Debugging

        internal class StateDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Transition[] Connections { get => state.Connections.ToArray(); }

            private readonly State state;

            public StateDebugView(State state)
            {
                this.state = state;
            }
        }

        #endregion Debugging
    }
}
