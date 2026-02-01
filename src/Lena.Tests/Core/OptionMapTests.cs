using Lena.Core;

namespace Lena.Tests.Core;

public class OptionMapTests
{
    [Fact]
    public void Map_on_Some_transforms_value()
    {
        var option = Option<int>.Some(21);

        var result = option.Map(x => x * 2);

        Assert.True(result.IsSome);
        Assert.Equal(42, result.Match(x => x, () => -1));
    }

    [Fact]
    public void Map_on_None_returns_None()
    {
        var option = Option<int>.None();

        var result = option.Map(x => x * 2);

        Assert.True(result.IsNone);
    }
}