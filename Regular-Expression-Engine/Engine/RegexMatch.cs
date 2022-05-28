using System.Diagnostics;

namespace Regular_Expression_Engine
{
    /// <summary>
    /// A found match in the input string.
    /// </summary>
    [DebuggerDisplay("Start = {Start}, End = {End}")]
    public class RegexMatch
    {
        /// <summary>
        /// The start index of the match.
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// The end index of the match.
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// Creates a new <see cref="RegexMatch"/>.
        /// </summary>
        /// <param name="start">The start index of the match.</param>
        /// <param name="end">The end index of the match.</param>
        public RegexMatch(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
