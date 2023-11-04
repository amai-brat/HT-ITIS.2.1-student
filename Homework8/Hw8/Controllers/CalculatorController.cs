using System.Diagnostics.CodeAnalysis;
using Hw8.Calculator;
using Hw8.Parser;
using Microsoft.AspNetCore.Mvc;
using static Hw8.Calculator.Messages;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate(
        [FromServices] ICalculator calculator,
        [FromServices] IParser parser,
        string val1,
        string operation,
        string val2)
    {
        var message = parser.TryParseArguments(val1, operation, val2, out var result);
        return message switch
        {
            DivisionByZeroMessage => BadRequest(DivisionByZeroMessage),
            InvalidNumberMessage => BadRequest(InvalidNumberMessage),
            InvalidOperationMessage => BadRequest(InvalidOperationMessage),
            _ => Ok(calculator.Calculate(result.FirstValue, result.Operation, result.SecondValue))
        };
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}