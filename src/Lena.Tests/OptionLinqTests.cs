using Lena.Core;

namespace Lena.Tests;

public class OptionLinqTests
{
    private static Option<int> Parse(string s) =>
        int.TryParse(s, out var i)
            ? Option<int>.Some(i)
            : Option<int>.None();

    [Fact]
    public void Linq_query_composes_options()
    {
        var result =
            from x in Parse("21")
            from y in Parse("21")
            select x + y;

        Assert.True(result.IsSome);
        Assert.Equal(42, result.Match(x => x, () => -1));
    }

    [Fact]
    public void Linq_query_short_circuits_on_None()
    {
        var result =
            from x in Parse("42")
            from y in Parse("abc")
            select x + y;

        Assert.True(result.IsNone);
    }
}