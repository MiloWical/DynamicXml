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
        ColonSymbol,
        QuestionMarkSymbol,
        Version,
        Identifier,
        Data,
        Eof
    }
}