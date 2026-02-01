using Lena.Core;

namespace Lena.Tests;

public class OptionBindTests
{
    private static Option<int> Parse(string s) =>
        int.TryParse(s, out var i)
            ? Option<int>.Some(i)
            : Option<int>.None();

    [Fact]
    public void Bind_flattens_Some()
    {
        var option = Option<string>.Some("42");

        var result = option.Bind(Parse);

        Assert.True(result.IsSome);
        Assert.Equal(42, result.Match(x => x, () => -1));
    }

    [Fact]
    public void Bind_on_None_returns_None()
    {
        var option = Option<string>.None();

        var result = option.Bind(Parse);

        Assert.True(result.IsNone);
    }
}