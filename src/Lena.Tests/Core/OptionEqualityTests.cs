using Lena.Core;

namespace Lena.Tests.Core;

public class OptionEqualityTests
{
    [Fact]
    public void None_is_equal_to_None()
    {
        var left = Option<int>.None();
        var right = Option<int>.None();

        Assert.True(left.Equals(right));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
    
    [Fact]
    public void Some_with_same_value_are_equal()
    {
        var left = Option<int>.Some(10);
        var right = Option<int>.Some(10);

        Assert.True(left.Equals(right));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
    
    [Fact]
    public void Some_with_different_values_are_not_equal()
    {
        var left = Option<int>.Some(10);
        var right = Option<int>.Some(20);

        Assert.False(left.Equals(right));
    }
    
    [Fact]
    public void Some_is_not_equal_to_None()
    {
        var some = Option<int>.Some(10);
        var none = Option<int>.None();

        Assert.False(some.Equals(none));
        Assert.False(none.Equals(some));
    }
    
    [Fact]
    public void Some_with_reference_type_uses_value_equality()
    {
        var left = Option<string>.Some("abc");
        var right = Option<string>.Some("abc");

        Assert.True(left.Equals(right));
    }
    
    private sealed class User
    {
        public int Id { get; }
        public User(int id) => Id = id;

        public override bool Equals(object? obj) =>
            obj is User other && Id == other.Id;

        public override int GetHashCode() => Id;
    }

    [Fact]
    public void Some_uses_custom_equality_of_value()
    {
        var left = Option<User>.Some(new User(1));
        var right = Option<User>.Some(new User(1));

        Assert.True(left.Equals(right));
    }
    
    [Fact]
    public void Equals_with_different_type_returns_false()
    {
        var option = Option<int>.Some(42);

        Assert.False(option.Equals("not an option"));
    }
    
    [Fact]
    public void None_equals_None_using_operator()
    {
        var a = Option<int>.None();
        var b = Option<int>.None();

        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Some_equals_Some_with_same_value_using_operator()
    {
        var a = Option<int>.Some(42);
        var b = Option<int>.Some(42);

        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Some_not_equals_Some_with_different_value_using_operator()
    {
        var a = Option<int>.Some(42);
        var b = Option<int>.Some(24);

        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Some_not_equals_None_using_operator()
    {
        var a = Option<int>.Some(42);
        var b = Option<int>.None();

        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void None_not_equals_Some_using_operator()
    {
        var a = Option<int>.None();
        var b = Option<int>.Some(42);

        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Equality_operator_is_consistent_with_Equals()
    {
        var a = Option<int>.Some(42);
        var b = Option<int>.Some(42);

        Assert.Equal(a.Equals(b), a == b);
        Assert.Equal(!a.Equals(b), a != b);
    }
}