using System.Linq.Expressions;
using Hw10.Services.TokenParser;

namespace Hw10.Services.ExpressionParser;

public interface IExpressionParser
{
    public Expression ParseExpressionFromTokens(IEnumerable<Token> tokens);
}