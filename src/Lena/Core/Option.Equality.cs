namespace Lena.Core;

public readonly partial struct Option<T> : IEquatable<Option<T>>
{
    public bool Equals(Option<T> other)
    {
        // None == None
        if (IsNone && other.IsNone)
            return true;

        // Some(x) == Some(y)
        if (IsSome && other.IsSome)
            return EqualityComparer<T>.Default.Equals(
                UnsafeValue,
                other.UnsafeValue);

        // Some != None
        return false;
    }

    public override bool Equals(object? obj)
    {
        return obj is Option<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return IsSome
            ? EqualityComparer<T>.Default.GetHashCode(UnsafeValue)
            : 0;
    }
    
    public static bool operator ==(Option<T> left, Option<T> right)
        => left.Equals(right);

    public static bool operator !=(Option<T> left, Option<T> right)
        => !left.Equals(right);
}