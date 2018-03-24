namespace DynamicXml.Scanner.Regex
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Token;

    public static class TokenRegexLookup
    {
        public static Dictionary<TokenType, Regex> Map { get; }

        static TokenRegexLookup()
        {
            //Use this to set the regex cache size to the number of 
            //token types to make the runtime code more efficient.
            //15 is the default maximum.
            Regex.CacheSize = Math.Max(15, Enum.GetNames(typeof(TokenType)).Length);

            //All regexes should begin at the beginning of the line s
            //because we clip the line out as we process.
            Map = new Dictionary<TokenType, Regex>
            {
                {TokenType.Comment, new Regex("\\A(<!\\-\\-)(.|\\s)*?(\\-\\->)")},
                {TokenType.CData, new Regex("\\A(<\\!\\[CDATA\\[)(.|\\s)*?(\\]\\]>)")},
                {TokenType.LessThanSymbol, new Regex("\\A<") },
                {TokenType.GreaterThanSymbol, new Regex("\\A>") },
                {TokenType.SlashSymbol, new Regex("\\A/") },
                {TokenType.EqualSymbol, new Regex("\\A=") },
                {TokenType.SingleQuoteSymbol, new Regex("\\A'") },
                {TokenType.DoubleQuoteSymbol, new Regex("\\A\"") },
                {TokenType.WhitespaceSymbol, new Regex("\\A\\s+") },
                {TokenType.ColonSymbol, new Regex("\\A:") },
                {TokenType.QuestionMarkSymbol, new Regex("\\A\\?") },
                {TokenType.Version, new Regex("\\A[0-9](\\.[0-9])*") },
                {TokenType.Identifier, new Regex("\\A[a-zA-Z0-9\\-_\\.]+") },
                {TokenType.Data, new Regex("\\A[a-zA-Z0-9&;\\-_\\\\/\\.#>\\?\\s]+") },
                {TokenType.Eof, new  Regex("\\z") }
            };
        }
    }
}