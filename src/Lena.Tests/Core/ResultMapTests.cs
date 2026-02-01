using Lena.Core;

namespace Lena.Tests.Core;

public class ResultMapTests
{
    [Fact]
    public void Map_on_Success_transforms_value()
    {
        var result = Result<int>.Success(21);
        
        var mapped = result.Map(x => x * 2);
        
        Assert.True(mapped.IsSuccess);
        Assert.Equal(42, mapped.Match(x => x, _ => -1));
    }
    
    [Fact]
    public void Map_on_Error_returns_Error()
    {
        var result = Result<int>.Error("test error");
        
        var mapped = result.Map(x => x * 2);
        
        Assert.True(mapped.IsError);
    }
    
    [Fact]
    public void Map_with_null_function_throws()
    {
        var result = Result<int>.Success(42);
        
        Assert.Throws<ArgumentNullException>(() =>
            result.Map<string>(null!));
    }
}
