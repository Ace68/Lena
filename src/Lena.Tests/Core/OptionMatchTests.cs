using Lena.Core;

namespace Lena.Tests.Core;

public class OptionMatchTests
{
    [Fact]
    public void Match_on_Some_executes_some_branch()
    {
        var option = Option<int>.Some(21);

        var result = option.Match(
            some: x => x * 2,
            none: () => 0);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Match_on_None_executes_none_branch()
    {
        var option = Option<int>.None();

        var result = option.Match(
            some: x => x * 2,
            none: () => 0);

        Assert.Equal(0, result);
    }
}