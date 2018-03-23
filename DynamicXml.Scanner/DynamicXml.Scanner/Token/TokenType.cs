namespace DynamicXml.Scanner.Token
{
    public enum TokenType
    {
        Unspecified,
        Undefined,
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