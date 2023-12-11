namespace Hw13.WebServer.Services.TokenParser;

public interface ITokenParser
{
    public List<Token> Parse(string input);
}