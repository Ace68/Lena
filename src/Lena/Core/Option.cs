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
    private T UnsafeValue => _value;

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
    
    public Option<TResult> Map<TResult>(Func<T, TResult> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));

        return IsSome
            ? Option<TResult>.Some(f(UnsafeValue))
            : Option<TResult>.None();
    }

    public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));

        return IsSome
            ? f(UnsafeValue)
            : Option<TResult>.None();
    }
    
    public TResult Match<TResult>(
        Func<T, TResult> some,
        Func<TResult> none)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));

        return IsSome
            ? some(UnsafeValue)
            : none();
    }

    public T GetOrElse(Func<T> fallback)
    {
        if (fallback is null) throw new ArgumentNullException(nameof(fallback));

        return IsSome
            ? UnsafeValue
            : fallback();
    }
    
    public T GetOrElse(T fallbackValue) =>
        IsSome ? UnsafeValue : fallbackValue;
    
    public T GetOrThrow() =>
        IsSome ? UnsafeValue : throw new InvalidOperationException("Option is None");
        
    public T GetOrThrow(string message) =>
        IsSome ? UnsafeValue : throw new InvalidOperationException(message);
        
    public T GetOrThrow(Exception exception) =>
        IsSome ? UnsafeValue : throw exception;
    
    public Option<T> Filter(Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        return IsSome && predicate(UnsafeValue) ? this : None();
    }
    
    public Option<T> Or(Option<T> alternative) =>
        IsSome ? this : alternative;
        
    public Option<T> Or(Func<Option<T>> alternativeProvider)
    {
        if (alternativeProvider is null) throw new ArgumentNullException(nameof(alternativeProvider));
        return IsSome ? this : alternativeProvider();
    }
    
    public bool Contains(T value) =>
        IsSome && EqualityComparer<T>.Default.Equals(UnsafeValue, value);
        
    public bool Exists(Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        return IsSome && predicate(UnsafeValue);
    }
    
    public void IfSome(Action<T> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));
        if (IsSome) action(UnsafeValue);
    }
    
    public void IfNone(Action action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));
        if (IsNone) action();
    }
    
    public T[] ToArray() =>
        IsSome ? new[] { UnsafeValue } : Array.Empty<T>();
        
    public List<T> ToList() =>
        IsSome ? new List<T> { UnsafeValue } : new List<T>();
        
    public IEnumerable<T> ToEnumerable()
    {
        if (IsSome) yield return UnsafeValue;
    }
}