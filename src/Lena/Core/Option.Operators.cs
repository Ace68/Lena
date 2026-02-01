namespace Lena.Core;

public partial struct Option<T>
{
    public static implicit operator Option<T>(T value) =>
        value is null ? None() : Some(value);
}