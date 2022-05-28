using System;
using System.Diagnostics;

namespace Regular_Expression_Engine
{
    [DebuggerDisplay("{ToString()}")]
    internal struct Token : IEquatable<Token>
    {
        public int Index;
        public char Character;
        public TokenType Type;

        public bool IsParenthesis => Type == TokenType.LeftParenthesis || Type == TokenType.RightParenthesis;

        public bool IsOperator => IsBinaryOperator || IsUnaryOperator;
        public bool IsUnaryOperator => Type == TokenType.UnaryPrefixOperator || Type == TokenType.UnaryPostfixOperator;
        public bool IsBinaryOperator => Type == TokenType.LeftAssociativeOperator || Type == TokenType.RightAssociativeOperator;

        public int OperatorPrecedence => 8 - GetPrecedencePriority();

        public Token(int index, TokenType type, char character)
        {
            Index = index;
            Character = character;
            Type = type;
        }

        public override string ToString()
        {
            bool isEscaped = (IsOperator || IsParenthesis) && Type == TokenType.Character;
            if (isEscaped == false && (IsOperator || IsParenthesis))
            { return $"{{{Character}}}"; }
            return isEscaped ? $"\\{Character}" : $"{Character}";
        }

        public static TokenType GetTypeFromChar(char character)
        {
            switch (character)
            {
                case '*':
                case '+':
                case '?':
                    return TokenType.UnaryPostfixOperator;
                case '|':
                case '&':
                    return TokenType.LeftAssociativeOperator;
                case '(':
                    return TokenType.LeftParenthesis;
                case ')':
                    return TokenType.RightParenthesis;
                case '.':
                    return TokenType.WildcardCharacter;
                default:
                    return TokenType.Character;
            }
        }

        private int GetPrecedencePriority()
        {
            //---------------------------------------------------------------------
            //|   | ERE Precedence (from high to low)    |                        |
            //---------------------------------------------------------------------
            //| 1 | Collation - related bracket symbols  | [==][::][..]           |
            //| 2 | Escaped characters                   | \< special character > |
            //| 3 | Bracket expression                   | []                     |
            //| 4 | Grouping                             | ()                     |
            //| 5 | Single - character - ERE duplication | * + ? {m, n}           |
            //| 6 | Concatenation                        | &                      |
            //| 7 | Anchoring                            | ^ $                    |
            //| 8 | Alternation                          | |                      |
            //---------------------------------------------------------------------

            switch (Character)
            {
                case '(':
                case ')':
                    return 4;
                case '*':
                case '+':
                case '?':
                    return 5;
                case '&':
                    return 6;
                case '|':
                    return 8;
                default: throw new InvalidOperationException();
            }
        }

        #region IEquatable

        public override int GetHashCode()
        {
            int hashCode = -671345384;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + Character.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj) => obj is Token token && Equals(token);
        public bool Equals(Token other) => Character == other.Character && Type == other.Type;

        public static bool operator ==(Token left, Token right) => left.Equals(right);
        public static bool operator !=(Token left, Token right) => !left.Equals(right);

        #endregion IEquatable
    }
}
