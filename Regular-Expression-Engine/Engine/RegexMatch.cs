using System.Diagnostics;

namespace Regular_Expression_Engine
{
    [DebuggerDisplay("Start = {Start}, End = {End}")]
    public class RegexMatch
    {
        public int Start { get; }
        public int End { get; }

        public RegexMatch(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
