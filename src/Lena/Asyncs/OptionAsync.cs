namespace Lena.Core;

/// <summary>
/// Async extensions for Option&lt;T&gt;.
/// </summary>
public static class OptionAsync
{
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Option<T> option,
        Func<T, Task<TResult>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        
        return option.IsNone 
            ? Option<TResult>.None()
            : Option<TResult>.Some(await f(option.Match(x => x, () => throw new InvalidOperationException())));
    }
    
    public static async Task<Option<TResult>> BindAsync<T, TResult>(
        this Option<T> option,
        Func<T, Task<Option<TResult>>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        
        return option.IsNone
            ? Option<TResult>.None()
            : await f(option.Match(x => x, () => throw new InvalidOperationException()));
    }
    
    public static async Task<TResult> MatchAsync<T, TResult>(
        this Option<T> option,
        Func<T, Task<TResult>> some,
        Func<Task<TResult>> none)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        
        return option.IsSome
            ? await some(option.Match(x => x, () => throw new InvalidOperationException()))
            : await none();
    }
}
