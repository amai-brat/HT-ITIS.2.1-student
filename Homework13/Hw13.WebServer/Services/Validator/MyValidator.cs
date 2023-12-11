using System.Globalization;
using Hw13.WebServer.ErrorMessages;

namespace Hw13.WebServer.Services.Validator;

public class MyValidator : IValidator
{
    private readonly char[] _operations = { '+', '-', '*', '/' };
    private readonly char[] _brackets = { '(', ')' };
    private readonly List<string> _errors = new();
    
    public string ValidateMathExpression(string? input)
    {
        return ValidateNotEmptiness(input) &&
               ValidateBrackets(input!) &&
               ValidateLackOfUnknownCharacters(input!) &&
               ValidateOperationPlace(input!) &&
               ValidateNumbersCorrectness(input!)
            ? string.Empty 
            : _errors.First();
    }

    private bool ValidateNotEmptiness(string? input)
    {
        if (!string.IsNullOrEmpty(input)) return true;
        
        _errors.Add(MathErrorMessager.EmptyString);
        return false;
    }
    
    private bool ValidateBrackets(string input)
    {
        var stack = new Stack<char>();
        foreach (var ch in input)
        {
            switch (ch)
            {
                case '(':
                    stack.Push(ch);
                    break;
                case ')' when stack.TryPeek(out var tail) && tail == '(':
                    stack.Pop();
                    break;
                case ')':
                    _errors.Add(MathErrorMessager.IncorrectBracketsNumber);
                    return false;
            }
        }

        if (stack.Count <= 0) return true;
        
        _errors.Add(MathErrorMessager.IncorrectBracketsNumber);
        return false;
    }

    private bool ValidateLackOfUnknownCharacters(string input)
    {
        foreach (var ch in input.Where(ch => 
                     !char.IsDigit(ch) && 
                     !char.IsWhiteSpace(ch) && 
                     !_operations.Contains(ch) &&
                     !_brackets.Contains(ch) &&
                     ch != '.'))
        {
            _errors.Add(MathErrorMessager.UnknownCharacterMessage(ch));
            return false;
        }

        return true;
    }

    private bool ValidateOperationPlace(string input)
    {
        var stack = new Stack<char>();
        foreach (var ch in input)
        {
            if (_operations.Contains(ch))
            {
                switch (stack.TryPeek(out var tail))
                {
                    case true when _operations.Contains(tail):
                        _errors.Add(MathErrorMessager.TwoOperationInRowMessage(tail.ToString(), ch.ToString()));
                        return false;
                    case true when tail == '(' && ch != '-':
                        _errors.Add(MathErrorMessager.InvalidOperatorAfterParenthesisMessage(ch.ToString()));
                        return false;
                    case false when ch != '-':
                        _errors.Add(MathErrorMessager.StartingWithOperation);
                        return false;
                    default:
                        stack.Push(ch);
                        break;
                }
            }
            else if (_brackets.Contains(ch))
            {
                switch (stack.TryPeek(out var tail))
                {
                    case true when ch == ')' && _operations.Contains(tail):
                        _errors.Add(MathErrorMessager.OperationBeforeParenthesisMessage(tail.ToString()));
                        return false;
                    default:
                        stack.Push(ch);
                        break;
                }
            }
            else if (!char.IsWhiteSpace(ch))
            {
                stack.Push(ch);
            }
        }

        if (_operations.Contains(stack.Pop()))
        {
            _errors.Add(MathErrorMessager.EndingWithOperation);
            return false;
        }

        return true;
    }

    private bool ValidateNumbersCorrectness(string input)
    {
        var numberStartPos = 0;
        var isPreviousDigit = false;
        
        for (var i = 0; i < input.Length; i++)
        {
            if (!char.IsDigit(input[i]) && !isPreviousDigit) continue;
            
            if (char.IsDigit(input[i]) && !isPreviousDigit)
            {
                numberStartPos = i;
                isPreviousDigit = true;
            }
            else if (char.IsDigit(input[i]) && isPreviousDigit || input[i] == '.')
            {
                isPreviousDigit = true;
            }
            else if (!char.IsDigit(input[i]) && isPreviousDigit)
            {
                var maybeNumber = input[numberStartPos..i];
                if (!double.TryParse(maybeNumber, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
                {
                    _errors.Add(MathErrorMessager.NotNumberMessage(maybeNumber));
                    return false;
                }

                isPreviousDigit = false;
            }
        }

        if (isPreviousDigit)
        {
            var maybeNumber = input[numberStartPos..input.Length];
            if (!double.TryParse(maybeNumber, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
            {
                _errors.Add(MathErrorMessager.NotNumberMessage(maybeNumber));
                return false;
            }
        }
        
        return true;
    }
}