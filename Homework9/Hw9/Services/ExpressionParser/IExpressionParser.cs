using System.Linq.Expressions;
using Hw9.Services.TokenParser;

namespace Hw9.Services.ExpressionParser;

public interface IExpressionParser
{
    public Expression ParseExpressionFromTokens(IEnumerable<Token> tokens);
}