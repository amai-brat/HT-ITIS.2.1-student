using System.Linq.Expressions;

namespace Hw10.Services.TokenParser;

public interface ITokenParser
{
    public List<Token> Parse(string input);
}