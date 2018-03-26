namespace DynamicXml.Scanner.Lexeme
{
    public enum LexemeType
    {
        Unspecified,
        Undefined,
        Comment,
        CData,
        LessThanSymbol,
        GreaterThanSymbol,
        SlashSymbol,
        EqualSymbol,
        SingleQuoteSymbol,
        DoubleQuoteSymbol,
        WhitespaceSymbol,
        OptionalWhitespaceSymbol,
        ColonSymbol,
        QuestionMarkSymbol,
        Version,
        Identifier,
        Data,
        Eof
    }
}