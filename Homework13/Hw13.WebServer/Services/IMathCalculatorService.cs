using Hw13.WebServer.Dto;

namespace Hw13.WebServer.Services;

public interface IMathCalculatorService
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}