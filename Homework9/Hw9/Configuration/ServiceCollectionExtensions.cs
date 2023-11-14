using Hw9.Services.ExpressionParser;
using Hw9.Services.MathCalculator;
using Hw9.Services.TokenParser;
using Hw9.Services.Validator;

namespace Hw9.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services
            .AddTransient<IMathCalculatorService, MathCalculatorService>()
            .AddTransient<IValidator, MyValidator>()
            .AddTransient<ITokenParser, MyTokenParser>()
            .AddTransient<IExpressionParser, MyExpressionParser>();
    }
}