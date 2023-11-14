using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using Hw9.Dto;
using Hw9.ErrorMessages;
using Hw9.Services.ExpressionParser;
using Hw9.Services.TokenParser;
using Hw9.Services.Validator;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    private readonly IValidator _validator;
    private readonly ITokenParser _tokenParser;
    private readonly IExpressionParser _expressionParser;

    public MathCalculatorService(IValidator validator, ITokenParser tokenParser, IExpressionParser expressionParser)
    {
        _validator = validator;
        _tokenParser = tokenParser;
        _expressionParser = expressionParser;
    }

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var error = _validator.ValidateMathExpression(expression);
        if (error != string.Empty)
        {
            return new CalculationMathExpressionResultDto(error);
        }
        
        var parsedTokens = _tokenParser.Parse(expression!);
        var exp = _expressionParser.ParseExpressionFromTokens(parsedTokens);

        var expVisitor = new MathExpressionVisitor();
        var executeBefore = expVisitor.GetExecuteBefore(exp);

        var result = await CalculateAsync(exp, executeBefore);
        return double.IsNaN(result) 
            ? new CalculationMathExpressionResultDto(MathErrorMessager.DivisionByZero)
            : new CalculationMathExpressionResultDto(result);
    }
    
    private async Task<double> CalculateAsync(Expression current, Dictionary<Expression, Tuple<Expression, Expression>> executeBefore)
    {
        if (!executeBefore.ContainsKey(current))
        {
            return double.Parse(current.ToString(), CultureInfo.InvariantCulture);
        }

        var leftTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalculateAsync(executeBefore[current].Item1, executeBefore);
        });
        var rightTask = Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await CalculateAsync(executeBefore[current].Item2, executeBefore);
        });

        var result = await Task.WhenAll(leftTask, rightTask);
        return current.NodeType switch
        {
            ExpressionType.Add => result[0] + result[1],
            ExpressionType.Subtract => result[0] - result[1],
            ExpressionType.Multiply => result[0] * result[1],
            ExpressionType.Divide when Math.Abs(result[1]) < double.Epsilon => double.NaN,
            ExpressionType.Divide => result[0] / result[1],
            _ => throw new InvalidOperationException("That expression type isn't supported")
        };
    }
}