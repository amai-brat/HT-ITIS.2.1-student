using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator;

public class MathExpressionVisitor : ExpressionVisitor
{
    private readonly Dictionary<Expression, Tuple<Expression, Expression>> _executeBefore = new();

    public Dictionary<Expression, Tuple<Expression, Expression>> GetExecuteBefore(Expression expression)
    {
        Visit(expression);
        return _executeBefore;
    }
    
    protected override Expression VisitBinary(BinaryExpression node)
    {
        _executeBefore.Add(node, new Tuple<Expression, Expression>(node.Left, node.Right));
        return base.VisitBinary(node);
    }
}