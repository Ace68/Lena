using Lena.Core;

namespace Lena.Tests;

public class OptionLawsTests
{
    [Fact]
    public void Map_identity_is_identity()
    {
        var option = Option<int>.Some(42);

        var result = option.Map(x => x);

        Assert.Equal(
            option.Match(x => x, () => -1),
            result.Match(x => x, () => -1));
    }
    
    [Fact]
    public void Bind_left_identity()
    {
        Option<int> f(int x) => Option<int>.Some(x * 2);

        var left = Option<int>.Some(42).Bind(f);
        var right = f(42);

        Assert.Equal(
            left.Match(x => x, () => -1),
            right.Match(x => x, () => -1));
    }
    
    [Fact]
    public void Bind_right_identity()
    {
        var option = Option<int>.Some(42);

        var result = option.Bind(Option<int>.Some);

        Assert.Equal(
            option.Match(x => x, () => -1),
            result.Match(x => x, () => -1));
    }
}