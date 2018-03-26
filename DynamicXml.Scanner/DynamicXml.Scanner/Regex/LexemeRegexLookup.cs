namespace DynamicXml.Scanner.Regex
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Lexeme;

    public static class LexemeRegexLookup
    {
        public static Dictionary<LexemeType, Regex> Map { get; }

        static LexemeRegexLookup()
        {
            //Use this to set the regex cache size to the number of 
            //lexeme types to make the runtime code more efficient.
            //15 is the default maximum.
            Regex.CacheSize = Math.Max(15, Enum.GetNames(typeof(LexemeType)).Length);

            //All regexes should begin at the beginning of the line s
            //because we clip the line out as we process.
            Map = new Dictionary<LexemeType, Regex>
            {
                {LexemeType.Comment, new Regex("\\A(<!\\-\\-)(.|\\s)*?(\\-\\->)")},
                {LexemeType.CData, new Regex("\\A(<\\!\\[CDATA\\[)(.|\\s)*?(\\]\\]>)")},
                {LexemeType.LessThanSymbol, new Regex("\\A<") },
                {LexemeType.GreaterThanSymbol, new Regex("\\A>") },
                {LexemeType.SlashSymbol, new Regex("\\A/") },
                {LexemeType.EqualSymbol, new Regex("\\A=") },
                {LexemeType.SingleQuoteSymbol, new Regex("\\A'") },
                {LexemeType.DoubleQuoteSymbol, new Regex("\\A\"") },
                {LexemeType.WhitespaceSymbol, new Regex("\\A\\s+") },
                {LexemeType.ColonSymbol, new Regex("\\A:") },
                {LexemeType.QuestionMarkSymbol, new Regex("\\A\\?") },
                {LexemeType.Version, new Regex("\\A[0-9](\\.[0-9])*") },
                {LexemeType.Identifier, new Regex("\\A[a-zA-Z0-9\\-_\\.]+") },
                {LexemeType.Data, new Regex("\\A[a-zA-Z0-9&;\\-_\\\\/\\.#>\\?\\s]+") },
                {LexemeType.Eof, new  Regex("\\z") }
            };
        }
    }
}