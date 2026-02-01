using Lena.Core;

namespace Lena.Tests.Core;

public class ResultCreationTests
{
    [Fact]
    public void Success_with_value_is_Success()
    {
        var result = Result<int>.Success(42);
        
        Assert.True(result.IsSuccess);
        Assert.False(result.IsError);
    }
    
    [Fact]
    public void Error_with_exception_is_Error()
    {
        var exception = new Exception("test error");
        var result = Result<int>.Error(exception);
        
        Assert.True(result.IsError);
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public void Error_with_message_is_Error()
    {
        var result = Result<int>.Error("test error");
        
        Assert.True(result.IsError);
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public void Success_with_null_throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Result<string>.Success(null!));
    }
    
    [Fact]
    public void Error_with_null_exception_throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Result<int>.Error((Exception)null!));
    }
}
