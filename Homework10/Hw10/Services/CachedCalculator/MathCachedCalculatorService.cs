using Hw10.DbModels;
using Hw10.Dto;
using Hw10.Services.MathCalculator;
using Microsoft.EntityFrameworkCore;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var cached = await _dbContext
			.SolvingExpressions
			.FirstOrDefaultAsync(expr => expr.Expression == expression);

		if (cached == null)
		{
			var result = await _simpleCalculator.CalculateMathExpressionAsync(expression);
			if (result.IsSuccess)
			{
				await _dbContext.SolvingExpressions.AddAsync(new SolvingExpression
				{
					Expression = expression!,
					Result = result.Result
				});
				await _dbContext.SaveChangesAsync();
			}

			return result;
		}
		
		await Task.Delay(1000);
		return new CalculationMathExpressionResultDto(cached.Result);
	}
}