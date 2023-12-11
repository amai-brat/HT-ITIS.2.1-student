using System.Linq.Expressions;
using Hw13.WebServer.Services.TokenParser;

namespace Hw13.WebServer.Services.ExpressionParser;

public interface IExpressionParser
{
    public Expression ParseExpressionFromTokens(IEnumerable<Token> tokens);
}