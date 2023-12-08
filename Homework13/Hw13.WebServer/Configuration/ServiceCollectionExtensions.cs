using Hw13.WebServer.Services;
using Hw13.WebServer.Services.CachedCalculator;
using Hw13.WebServer.Services.ExpressionParser;
using Hw13.WebServer.Services.MathCalculator;
using Hw13.WebServer.Services.TokenParser;
using Hw13.WebServer.Services.Validator;

namespace Hw13.WebServer.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services
            .AddTransient<IValidator, MyValidator>()
            .AddTransient<ITokenParser, MyTokenParser>()
            .AddTransient<IExpressionParser, MyExpressionParser>()
            .AddTransient<MathCalculatorService>();
    }
    
    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        return services.AddScoped<IMathCalculatorService>(s =>
            new MathCachedCalculatorService(
                s.GetRequiredService<MathCalculatorService>()));
    }
}