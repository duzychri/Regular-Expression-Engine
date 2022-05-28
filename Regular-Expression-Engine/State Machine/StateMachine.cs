using System;
using System.Linq;
using System.Collections.Generic;

namespace Regular_Expression_Engine
{
    internal class StateMachine
    {
        public State Start { get; }

        #region Constructor & Builder

        private StateMachine(State start)
        {
            Start = start;
        }

        public static StateMachine Build(IEnumerable<Token> tokens)
        {
            Stack<Fragment> fragments = new Stack<Fragment>();
            foreach (Token token in tokens)
            {
                // Character
                if (token.Type == TokenType.Character)
                {
                    State start = new State($"Character Start ({token.Character})");
                    State end = new State($"Character End ({token.Character})");
                    start.AddConnection(end, token.Character);

                    Fragment fragment = new Fragment(start, end);
                    fragments.Push(fragment);
                }
                // Wildcard character
                else if (token.Type == TokenType.WildcardCharacter)
                {
                    State start = new State($"Wildcard character Start ({token.Character})");
                    State end = new State($"Wildcard character End ({token.Character})");
                    start.AddAnyConnection(end);

                    Fragment fragment = new Fragment(start, end);
                    fragments.Push(fragment);
                }
                // Alternation
                else if (token.IsOperator && token.Character == '|')
                {
                    State start = new State("| Start");
                    State end = new State("| End");

                    Fragment b = fragments.Pop();
                    Fragment a = fragments.Pop();

                    start.AddEmptyConnection(a.Start);
                    start.AddEmptyConnection(b.Start);

                    a.End.AddEmptyConnection(end);
                    b.End.AddEmptyConnection(end);

                    Fragment fragment = new Fragment(start, end);
                    fragments.Push(fragment);
                }
                // Concatenation
                else if (token.IsOperator && token.Character == '&')
                {
                    Fragment b = fragments.Pop();
                    Fragment a = fragments.Pop();

                    a.End.AddEmptyConnection(b.Start);

                    Fragment fragment = new Fragment(a.Start, b.End);
                    fragments.Push(fragment);
                }
                // Zero or one
                else if (token.IsOperator && token.Character == '?')
                {
                    Fragment a = fragments.Pop();

                    State start = new State("? Start");
                    State end = new State("? End");

                    start.AddEmptyConnection(a.Start);
                    a.End.AddEmptyConnection(end);
                    start.AddEmptyConnection(end);

                    Fragment fragment = new Fragment(start, end);
                    fragments.Push(fragment);
                }
                // Zero or more
                else if (token.IsOperator && token.Character == '*')
                {
                    Fragment a = fragments.Pop();
                    a.End.AddEmptyConnection(a.Start);

                    State start = new State("* Start");
                    State end = new State("* End");

                    start.AddEmptyConnection(a.Start);
                    a.End.AddEmptyConnection(end);
                    start.AddEmptyConnection(end);

                    Fragment fragment = new Fragment(start, end);
                    fragments.Push(fragment);
                }
                // One or more
                else if (token.IsOperator && token.Character == '+')
                {
                    Fragment a = fragments.Pop();
                    a.End.AddEmptyConnection(a.Start);

                    State start = new State("+ Start");
                    State end = new State("+ End");

                    start.AddEmptyConnection(a.Start);
                    a.End.AddEmptyConnection(end);

                    Fragment fragment = new Fragment(start, end);
                    fragments.Push(fragment);
                }
            }

            if (fragments.Count > 1) { throw new InvalidOperationException(); }
            State startState = fragments.First().Start;
            StateMachine stateMachine = new StateMachine(startState);
            return stateMachine;
        }

        #endregion Constructor & Builder

        internal (bool success, int endIndex) Evaluate(string input, int startIndex, bool isCaseSensitive)
        {
            return Evaluate(Start, startIndex);

            (bool success, int endIndex) Evaluate(State state, int index)
            {
                if (state.IsEndState)
                { return (true, index); }

                foreach (Transition connection in state.Connections)
                {
                    if (index < input.Length && CompareCharacters(connection.Character, input[index]))
                    {
                        (bool success, int endIndex) = Evaluate(connection.State, index + 1);
                        if (success)
                        { return (true, endIndex); }
                    }
                    else if (index < input.Length && connection.IsAnyCharacter)
                    {
                        (bool success, int endIndex) = Evaluate(connection.State, index + 1);
                        if (success)
                        { return (true, endIndex); }
                    }
                    else if (connection.IsEmpty)
                    {
                        (bool success, int endIndex) = Evaluate(connection.State, index);
                        if (success)
                        { return (true, endIndex); }
                    }
                }

                return (false, index);
            }

            bool CompareCharacters(char a, char b)
            {
                if (isCaseSensitive == false)
                {
                    a = char.ToLowerInvariant(a);
                    b = char.ToLowerInvariant(b);
                }
                return a == b;
            }
        }
    }
}
