namespace Regular_Expression_Engine
{
    internal struct Fragment
    {
        public State Start { get; set; }
        public State End { get; set; }

        public Fragment(State start, State end)
        {
            Start = start;
            End = end;
        }
    }
}
