using Lena.Core;

namespace Lena.Tests.Core;

public class OptionCreationTests
{
    [Fact]
    public void Some_with_value_is_Some()
    {
        var option = Option<int>.Some(42);

        Assert.True(option.IsSome);
        Assert.False(option.IsNone);
    }

    [Fact]
    public void None_is_None()
    {
        var option = Option<int>.None();

        Assert.True(option.IsNone);
        Assert.False(option.IsSome);
    }

    [Fact]
    public void Some_with_null_throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Option<string>.Some(null!));
    }
}