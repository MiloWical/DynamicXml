namespace DynamicXml.Scanner.Token
{
    public enum TokenType
    {
        Undefined,
        LessThanSymbol,
        GreaterThanSymbol,
        SlashSymbol,
        EqualSymbol,
        SingleQuoteSymbol,
        DoubleQuoteSymbol,
        WhitespaceSymbol,
        ColonSymbol,
        Identifier,
        Data,
        Eof
    }
}