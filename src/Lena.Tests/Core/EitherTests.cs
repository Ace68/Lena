using Lena.Core;

namespace Lena.Tests.Core;

public class EitherTests
{
    [Fact]
    public void Left_value_is_Left()
    {
        var either = Either<string, int>.Left("error");
        
        Assert.True(either.IsLeft);
        Assert.False(either.IsRight);
    }
    
    [Fact]
    public void Right_value_is_Right()
    {
        var either = Either<string, int>.Right(42);
        
        Assert.True(either.IsRight);
        Assert.False(either.IsLeft);
    }
    
    [Fact]
    public void Map_on_Right_transforms_value()
    {
        var either = Either<string, int>.Right(21);
        
        var result = either.Map(x => x * 2);
        
        Assert.True(result.IsRight);
        Assert.Equal(42, result.Match(_ => -1, x => x));
    }
    
    [Fact]
    public void Map_on_Left_returns_Left()
    {
        var either = Either<string, int>.Left("error");
        
        var result = either.Map(x => x * 2);
        
        Assert.True(result.IsLeft);
        Assert.Equal("error", result.Match(x => x, _ => ""));
    }
    
    [Fact]
    public void Bind_on_Right_continues_chain()
    {
        var either = Either<string, int>.Right(42);
        
        var result = either.Bind(x => Either<string, string>.Right(x.ToString()));
        
        Assert.True(result.IsRight);
        Assert.Equal("42", result.Match(_ => "", x => x));
    }
    
    [Fact]
    public void Bind_on_Left_returns_Left()
    {
        var either = Either<string, int>.Left("error");
        
        var result = either.Bind(x => Either<string, string>.Right(x.ToString()));
        
        Assert.True(result.IsLeft);
        Assert.Equal("error", result.Match(x => x, _ => ""));
    }
}
