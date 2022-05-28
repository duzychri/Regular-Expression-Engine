using System;
using System.Linq;
using System.Collections.Generic;

namespace Regular_Expression_Engine
{
    internal static class Tokenizer
    {
        internal static IEnumerable<Token> TokenizeExpression(string expression)
        {
            int index = 0;
            bool isEscaping = false;
            foreach (char curr in expression)
            {
                if (isEscaping)
                {
                    if (curr != '\\' && (Token.GetTypeFromChar(curr) == TokenType.Character || curr == '&'))
                    { throw new InvalidOperationException($"Invalid escape sequence: The character {curr} can not be escaped."); }

                    yield return new Token(index, TokenType.Character, curr);
                    isEscaping = false;
                }
                else if (curr == '\\')
                {
                    isEscaping = true;
                }
                else if (curr == '&')
                {
                    yield return new Token(index, TokenType.Character, curr);
                }
                else
                {
                    yield return new Token(index, Token.GetTypeFromChar(curr), curr);
                }
                index++;
            }
        }

        internal static IEnumerable<Token> AddConcatenations(IEnumerable<Token> tokens)
        {
            Token previous = tokens.First();
            yield return tokens.First();
            foreach (Token current in tokens.Skip(1))
            {
                if (previous.Character != '&' &&
                    previous.Type != TokenType.LeftParenthesis &&
                    current.Type != TokenType.RightParenthesis &&
                    previous.IsBinaryOperator == false &&
                    current.IsOperator == false
                    //(previous.Type == TokenType.Normal || current.Type == TokenType.Normal)
                    )
                {
                    yield return new Token(-1, TokenType.LeftAssociativeOperator, '&');
                }
                yield return current;
                previous = current;
            }
        }

        /// <remarks>
        /// https://www.andr.mu/logs/the-shunting-yard-algorithm/
        /// </remarks>
        internal static IEnumerable<Token> ConvertToPostfixNotation(IEnumerable<Token> tokens)
        {
            Stack<Token> operatorStack = new Stack<Token>();

            // Given a string mathematical expression, from left to right, scan for each token.
            foreach (Token token in tokens)
            {
                // If token is an operand, push it onto output.
                if (token.Type == TokenType.Character || token.Type == TokenType.WildcardCharacter)
                {
                    yield return token;
                }
                // If token is a unary postfix operator, push it onto output.
                else if (token.Type == TokenType.UnaryPostfixOperator)
                {
                    yield return token;
                }
                // If token is a unary prefix operator, push it onto stack.
                else if (token.Type == TokenType.UnaryPrefixOperator)
                {
                    operatorStack.Push(token);
                }
                // If token is a function, push it onto stack. (Ignored)
                // If operator is an left-associative operator (i.e. +, -, *, /):
                if (token.Type == TokenType.LeftAssociativeOperator)
                {
                    // While stack contains an operator that has equal or higher precedence than token, pop stack and push the popped item onto output.
                    while (operatorStack.Any() && operatorStack.Peek().IsOperator
                        && operatorStack.Peek().OperatorPrecedence >= token.OperatorPrecedence)
                    {
                        yield return operatorStack.Pop();
                    }

                    // Push token onto stack.
                    operatorStack.Push(token);
                }
                // If operator is an right-associative operator (i.e. ^, EE, √, -):
                else if (token.Type == TokenType.RightAssociativeOperator)
                {
                    // While stack contains an operator that has higher precedence than token, pop stack and push popped item onto output.
                    while (operatorStack.Any() && operatorStack.Peek().IsOperator
                        && operatorStack.Peek().OperatorPrecedence > token.OperatorPrecedence)
                    {
                        yield return operatorStack.Pop();
                    }

                    // Push token onto stack.
                    operatorStack.Push(token);
                }
                // If token is left-parenthesis (:
                else if (token.Type == TokenType.LeftParenthesis)
                {
                    // Push token onto stack.
                    operatorStack.Push(token);
                }
                // If token is right-parenthesis ):
                else if (token.Type == TokenType.RightParenthesis)
                {
                    // While top of stack is not a left-parenthesis, pop each item from stack and push each popped item onto output.
                    while (operatorStack.Peek().Type != TokenType.LeftParenthesis)
                    { yield return operatorStack.Pop(); }
                    // Now pop stack one more time (popped item should be a left-parenthesis).
                    operatorStack.Pop();

                    // If top of stack is now a function, pop that as well and push it onto output. (Ingored)
                }
            }

            // Pop remainder of stack and push each popped item onto output.
            while (operatorStack.Any())
            {
                Token token = operatorStack.Pop();
                if (token.Type == TokenType.LeftParenthesis)
                { throw new InvalidOperationException($"Mismatched parentheses at index {token.Index}."); }
                yield return token;
            }
        }
    }
}
