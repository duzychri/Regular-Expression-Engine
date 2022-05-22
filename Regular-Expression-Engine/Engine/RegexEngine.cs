using System;
using System.Collections.Generic;

namespace Regular_Expression_Engine
{
    /// <summary>
    /// A regular expression engine.
    /// </summary>
    /// <remarks>
    /// Supported operators:<br/>
    /// - Alternation '|'<br/>
    /// - Zero or one '?'<br/>
    /// - Zero or more '*'<br/>
    /// - One or more '+'<br/>
    /// - Parenthesis '(', ')'<br/>
    /// Operators can be escaped using \.
    /// </remarks>
    public class RegexEngine
    {
        private readonly bool isCaseSensitive;
        private readonly StateMachine stateMachine;

        /// <summary>
        /// Creates a new <see cref="RegexEngine"/>.
        /// </summary>
        /// <param name="expression">The expression to create a state machine for.</param>
        /// <param name="isCaseSensitive">If <c>false</c> then the matching will ignore the case of the character.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="expression"/> is null or empty.</exception>
        public RegexEngine(string expression, bool isCaseSensitive = true)
        {
            if (string.IsNullOrWhiteSpace(expression)) { throw new ArgumentNullException(nameof(expression), $"{nameof(expression)} is null or empty."); }

            // Tokenize expression.
            var tokens = Tokenizer.TokenizeExpression(expression);
            // Add explicit concatenations
            tokens = Tokenizer.AddConcatenations(tokens);
            // Convert tokens-string to polish/postfix notation.
            tokens = Tokenizer.ConvertToPostfixNotation(tokens);
            // Build state machine.
            stateMachine = StateMachine.Build(tokens);
            // Set options.
            this.isCaseSensitive = isCaseSensitive;
        }

        /// <summary>
        /// Checks an input string for possible matches.
        /// </summary>
        /// <param name="input">The input string to check for matches.</param>
        /// <returns>An array of <see cref="RegexMatch"/>. If no matches were found the array is empty.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="input"/> is null or empty.</exception>
        public RegexMatch[] Match(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentNullException(nameof(input), $"{nameof(input)} is null or empty."); }

            List<RegexMatch> matches = new List<RegexMatch>();

            for (int start = 0; start < input.Length; start++)
            {
                (bool success, int end) = stateMachine.Evaluate(input, start, isCaseSensitive);
                if (success && end - start > 0)
                {
                    RegexMatch match = new RegexMatch(start, end);
                    matches.Add(match);
                }
            }

            return matches.ToArray();
        }
    }
}