﻿namespace DynamicXml.Scanner.Token
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class TokenRegexLookup
    {
        public static Dictionary<TokenType, Regex> Map { get; }

        static TokenRegexLookup()
        {
            //Use this to set the regex cache size to the number of 
            //token types to make the runtime code more efficient.
            //15 is the default maximum.
            Regex.CacheSize = Math.Max(15, Enum.GetNames(typeof(TokenType)).Length);

            Map = new Dictionary<TokenType, Regex>
            {
                {TokenType.LessThanSymbol, new Regex("\\A<") },
                {TokenType.GreaterThanSymbol, new Regex("\\A>") },
                {TokenType.SlashSymbol, new Regex("\\A/") },
                {TokenType.EqualSymbol, new Regex("\\A=") },
                {TokenType.SingleQuoteSymbol, new Regex("\\A'") },
                {TokenType.DoubleQuoteSymbol, new Regex("\\A\"") },
                {TokenType.WhitespaceSymbol, new Regex("\\A\\s+") },
                {TokenType.ColonSymbol, new Regex("\\A:") },
                {TokenType.Identifier, new Regex("\\A[a-zA-Z0-9]-_\\.") },
                {TokenType.Data, new Regex("\\A[a-zA-Z0-9]&;-_\\\\/\\.#>'\"") },
                {TokenType.Eof, new  Regex("\\z") }
            };
        }
    }
}