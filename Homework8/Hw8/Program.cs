using System.Diagnostics.CodeAnalysis;
using Hw8.Calculator;
using Hw8.Parser;

namespace Hw8;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
            options.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;
        });
        
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IParser, MyParser>();
        builder.Services.AddSingleton<ICalculator, MyCalculator>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseMiniProfiler();
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Calculator}/{action=Index}");

        app.Run();
    }
}