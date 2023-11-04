namespace Hw8.Parser;

public interface IParser
{
    public string TryParseArguments(string? val1, string? operation, string val2, 
        out ParseResult parseResult);
}