using Lena.Core;

namespace Lena.Tests.Core;

public class ResultBindTests
{
    private static Result<int> Parse(string s) =>
        int.TryParse(s, out var i)
            ? Result<int>.Success(i)
            : Result<int>.Error($"Cannot parse '{s}' as integer");
    
    [Fact]
    public void Bind_on_Success_continues_chain()
    {
        var result = Result<string>.Success("42");
        
        var bound = result.Bind(Parse);
        
        Assert.True(bound.IsSuccess);
        Assert.Equal(42, bound.Match(x => x, _ => -1));
    }
    
    [Fact]
    public void Bind_on_Error_returns_Error()
    {
        var result = Result<string>.Error("initial error");
        
        var bound = result.Bind(Parse);
        
        Assert.True(bound.IsError);
    }
    
    [Fact]
    public void Bind_with_failing_function_returns_Error()
    {
        var result = Result<string>.Success("not-a-number");
        
        var bound = result.Bind(Parse);
        
        Assert.True(bound.IsError);
    }
}
