namespace Lena.Core;

/// <summary>
/// Functor             -> Map
/// Monad               -> Bind
/// Pattern matching    -> Match
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly partial struct Option<T>
{
    private readonly T _value;

    public bool IsSome { get; }
    public bool IsNone => !IsSome;

    private Option(T value, bool isSome)
    {
        _value = value;
        IsSome = isSome;
    }

    public static Option<T> Some(T value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return new Option<T>(value, true);
    }

    public static Option<T> None() =>
        new Option<T>(default!, false);

    // Unsafe access (interno)
    internal T Value =>
        IsSome
            ? _value
            : throw new InvalidOperationException("Option has no value");
    
    public Option<TResult> Map<TResult>(Func<T, TResult> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));

        return IsSome
            ? Option<TResult>.Some(f(_value))
            : Option<TResult>.None();
    }

    public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));

        return IsSome
            ? f(_value)
            : Option<TResult>.None();
    }
    
    public TResult Match<TResult>(
        Func<T, TResult> some,
        Func<TResult> none)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));

        return IsSome
            ? some(_value)
            : none();
    }

    public T GetOrElse(Func<T> fallback)
    {
        if (fallback is null) throw new ArgumentNullException(nameof(fallback));

        return IsSome
            ? _value
            : fallback();
    }
}