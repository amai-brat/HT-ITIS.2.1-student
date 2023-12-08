using Hw13.WebServer.Dto;

namespace Hw13.WebServer.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private static readonly Dictionary<string, double> Cache = new();
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(IMathCalculatorService simpleCalculator)
	{
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		if (expression != null && Cache.TryGetValue(expression, out var cached))
		{
			return new CalculationMathExpressionResultDto(cached);
		}
		
		var result = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (result.IsSuccess)
		{
			Cache.TryAdd(expression!, result.Result);
		}

		return result;
	}
}