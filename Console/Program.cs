#pragma warning disable IDE0051 // Remove unused private members

internal class Program
{
    public static void Main()
    {
        TestParameters[] tests = {
            new()
            {
                Regex = "a(bc)+e",
                MatchingStrings = new string[] { "234234abcbcbcbce345", "abce" },
                NonMatchingStrings = new string[] { "a", "bbbbb", "abe", "abc", "bce", "ae" },
            },
            new()
            {
                Regex = "a(bc)*e",
                MatchingStrings = new string[] { "234234abcbcbcbce345", "abce", "ae" },
                NonMatchingStrings = new string[] { "a", "bbbbb", "abe", "abc", "bce" },
            },
            new()
            {
                Regex = "a(b|c)*e",
                MatchingStrings = new string[] { "234234abcbcbcbce345", "abce", "ae", "abe", "ace" },
                NonMatchingStrings = new string[] { "a", "bbbbb",  "abc", "bce" },
            },
            new()
            {
                Regex = "A(B|C)*e",
                IsCaseSensitive = false,
                MatchingStrings = new string[] { "234234abcbcbcbce345", "abce", "ae", "abe", "ace" },
                NonMatchingStrings = new string[] { "a", "bbbbb",  "abc", "bce" },
            },
            new()
            {
                Regex = ".+",
                IsCaseSensitive = false,
                MatchingStrings = new string[] { "234234abcbcbcbce345", "abce", "ae", "abe", "ace", "a", "bbbbb",  "abc", "bce" },
                NonMatchingStrings = new string[] {},
            },
            new()
            {
                Regex = "\\.+",
                IsCaseSensitive = false,
                MatchingStrings = new string[] { "......" },
                NonMatchingStrings = new string[] { "234234abcbcbcbce345", "abce", "ae", "abe", "ace", "a", "bbbbb",  "abc", "bce" },
            }
        };

        //RegexEngine engine2 = new RegexEngine("a*", isCaseSensitive: false);
        //var test5234 = engine2.Match(" asd asd  asd ");

        foreach (TestParameters value in tests)
        {
            RegexEngine engine = new RegexEngine(value.Regex, isCaseSensitive: value.IsCaseSensitive);
            foreach (string matchingString in value.MatchingStrings)
            {
                RegexMatch[] matches = engine.Match(matchingString);
                if (matches.Any() == false) { Console.WriteLine($"Waring: String '{matchingString}' does not match regex '{value.Regex}', but should."); }
            }
            foreach (string matchingString in value.NonMatchingStrings)
            {
                RegexMatch[] matches = engine.Match(matchingString);
                if (matches.Any()) { Console.WriteLine($"Waring: String '{matchingString}' does match regex '{value.Regex}', but shouldn't."); }
            }
        }

        Console.WriteLine("Tests are done!");
        Console.ReadLine();
    }

    private static void WriteEnumerable<T>(IEnumerable<T> source)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(source.First());
        foreach (T item in source.Skip(1))
        {
            sb.Append(", ");
            sb.Append(item);
        }
        Console.WriteLine(sb.ToString());
    }
}
