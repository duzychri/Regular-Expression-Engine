namespace Regular_Expression_Engine
{
    internal enum TokenType : byte
    {
        Character,
        WildcardCharacter,
        LeftParenthesis,
        RightParenthesis,
        UnaryPrefixOperator,
        UnaryPostfixOperator,
        LeftAssociativeOperator,
        RightAssociativeOperator,        
    }
}
