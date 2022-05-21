namespace Regular_Expression_Engine
{
    internal enum TokenType : byte
    {
        Character,
        LeftParenthesis,
        RightParenthesis,
        UnaryPrefixOperator,
        UnaryPostfixOperator,
        LeftAssociativeOperator,
        RightAssociativeOperator,
    }
}
