namespace DynamicXml.Scanner.Token
{
    public class ScannedToken
    {
        public TokenType Type { get; }
        public string Literal { get; }

        public ScannedToken(TokenType tokenType, string tokenLiteral)
        {
            Type = tokenType;
            if (tokenLiteral != null) Literal = tokenLiteral;
        }
    }
}
