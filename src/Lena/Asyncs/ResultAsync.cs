using Lena.Core;

namespace Lena.Asyncs;

/// <summary>
/// Async extensions for Result&lt;T&gt;.
/// </summary>
public static class ResultAsync
{
    public static async Task<Result<TResult>> MapAsync<T, TResult>(
        this Result<T> result,
        Func<T, Task<TResult>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        
        return result.IsError
            ? Result<TResult>.Error(result.Match(_ => throw new InvalidOperationException(), ex => ex))
            : Result<TResult>.Success(await f(result.Match(x => x, _ => throw new InvalidOperationException())));
    }
    
    public static async Task<Result<TResult>> BindAsync<T, TResult>(
        this Result<T> result,
        Func<T, Task<Result<TResult>>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        
        return result.IsError
            ? Result<TResult>.Error(result.Match(_ => throw new InvalidOperationException(), ex => ex))
            : await f(result.Match(x => x, _ => throw new InvalidOperationException()));
    }
    
    public static async Task<TResult> MatchAsync<T, TResult>(
        this Result<T> result,
        Func<T, Task<TResult>> success,
        Func<Exception, Task<TResult>> error)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (error is null) throw new ArgumentNullException(nameof(error));
        
        return result.IsSuccess
            ? await success(result.Match(x => x, _ => throw new InvalidOperationException()))
            : await error(result.Match(_ => throw new InvalidOperationException(), ex => ex));
    }
    
    public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        
        try
        {
            return Result<T>.Success(await f());
        }
        catch (Exception ex)
        {
            return Result<T>.Error(ex);
        }
    }
}
