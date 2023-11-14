using System.Linq.Expressions;

namespace Hw9.Services.TokenParser;

public interface ITokenParser
{
    public List<Token> Parse(string input);
}