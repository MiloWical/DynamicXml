namespace DynamicXml.Scanner.TokenProvider
{
    #region Imports

    using Token;

    #endregion

    public interface ITokenReader
    {
        ScannedToken GetNextTokenFromBuffer(TokenType specifiedToken);
    }
}
