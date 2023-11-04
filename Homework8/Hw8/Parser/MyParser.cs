using System.Globalization;
using Hw8.Calculator;

namespace Hw8.Parser;

public class MyParser : IParser
{
    public string TryParseArguments(string? val1, string? operation, string val2, out ParseResult parseResult)
    {
        parseResult = new ParseResult();
        if (!double.TryParse(val1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value1) ||
            !double.TryParse(val2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value2))
        {
            return Messages.InvalidNumberMessage;
        }
        parseResult.FirstValue = value1;
        parseResult.SecondValue = value2;
        
        var op = ParseOperation(operation);
        parseResult.Operation = op;
        switch (op)
        {
            case Operation.Invalid:
                return Messages.InvalidOperationMessage;
            case Operation.Divide when value2 < double.Epsilon:
                return Messages.DivisionByZeroMessage;
            case Operation.Plus:
            case Operation.Minus:
            case Operation.Multiply:
            case Operation.Divide:
            default:
                return Messages.Ok;
        }
    }

    private static Operation ParseOperation(string? operation)
    {
        if (operation == null)
            return Operation.Invalid;

        return operation.ToLower() switch
        {
            "plus" => Operation.Plus,
            "minus" => Operation.Minus,
            "multiply" => Operation.Multiply,
            "divide" => Operation.Divide,
            _ => Operation.Invalid
        };
    }
}