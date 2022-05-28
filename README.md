# Regular-Expression-Engine
A simple regular expression engine written in C#.

The engine tokenizes an input regular expression string, then converts it into postfix notation and uses this to build a non-deterministic automata that can be recursivly moved through to check an input string for matches.

## Supported operators

| Operation | Operator | Description |
| ----------- | ----------- | ----------- |
| Alternation | \| | Matches the character left or right of the \|. |
| Zero or one | ? | Matches the character before the ? zero or one times. |
| Zero or more | \* | Matches the character before the * zero or more times. |
| One or more | + | Matches the character before the + one or more times. |
| Parenthesis | ( and ) | Allows the grouping of values inside the parenthesis. |
| Escape | \ | scapes the operator before the / (including itself). |

## Supported special characters

| Type | Character | Description |
| ----------- | ----------- | ----------- |
| Wildcard | . | Matches any character. |

## How to use

```csharp
string expression = "a(bc)+e";
string text = "Lore ipsum...";
RegexEngine engine = new RegexEngine(expression);
RegexMatch[] matches = engine.Match(text);
```