using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using Hw10.Dto;
using Hw10.ErrorMessages;
using Hw10.Services.ExpressionParser;
using Hw10.Services.TokenParser;
using Hw10.Services.Validator;

namespace Hw10.Services.MathCalculator;

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
    
    private async Task<double> CalculateAsync(Expression current, Dictionary<Expression, Tuple<Expression, Expression?>> executeBefore)
    {
        if (!executeBefore.ContainsKey(current))
        {
            return double.Parse(current.ToString(), CultureInfo.InvariantCulture);
        }

        await Task.Delay(1000);

        if (executeBefore[current].Item2 == null)
        {
            return -1 * await CalculateAsync(executeBefore[current].Item1, executeBefore);
        }
        
        var leftTask = Task.Run(async () => 
            await CalculateAsync(executeBefore[current].Item1, executeBefore));
        var rightTask = Task.Run(async () => 
            await CalculateAsync(executeBefore[current].Item2!, executeBefore));

        var result = await Task.WhenAll(leftTask, rightTask);
        return Calculate(result[0], current.NodeType, result[1]);

    }

    [ExcludeFromCodeCoverage]
    private static double Calculate(double arg1, ExpressionType type, double arg2)
    {
        return type switch
        {
            ExpressionType.Add => arg1 + arg2,
            ExpressionType.Subtract => arg1 - arg2,
            ExpressionType.Multiply => arg1 * arg2,
            ExpressionType.Divide when Math.Abs(arg2) < double.Epsilon => double.NaN,
            ExpressionType.Divide => arg1 / arg2,
            _ => throw new InvalidOperationException("That expression type isn't supported")
        };
    }
}