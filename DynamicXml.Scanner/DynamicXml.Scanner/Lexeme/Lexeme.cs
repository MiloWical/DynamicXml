﻿namespace DynamicXml.Scanner.Lexeme
{
    public class Lexeme
    {
        public LexemeType Type { get; }
        public string Literal { get; set; }

        public Lexeme(LexemeType lexemeType, string lexemeLiteral)
        {
            Type = lexemeType;
            if (lexemeLiteral != null) Literal = lexemeLiteral;
        }
    }
}
