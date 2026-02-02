namespace Lena.Core;

/// <summary>
/// Represents a result that can either be a success value or an error.
/// Inspired by Rust's Result and F#'s Result types.
/// </summary>
/// <typeparam name="T">The type of the success value</typeparam>
public readonly partial struct Result<T>
{
    private readonly T _value;
    private readonly Exception _error;
    
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    
    private T UnsafeValue => _value;
    private Exception UnsafeError => _error;
    
    private Result(T value, Exception error, bool isSuccess)
    {
        _value = value;
        _error = error;
        IsSuccess = isSuccess;
    }
    
    public static Result<T> Success(T value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));
        return new Result<T>(value, null!, true);
    }
    
    public static Result<T> Error(Exception error)
    {
        if (error is null)
            throw new ArgumentNullException(nameof(error));
        return new Result<T>(default!, error, false);
    }
    
    public static Result<T> Error(string message) =>
        Error(new Exception(message));
    
    public Result<TResult> Map<TResult>(Func<T, TResult> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        return IsSuccess
            ? Result<TResult>.Success(f(UnsafeValue))
            : Result<TResult>.Error(UnsafeError);
    }
    
    public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        return IsSuccess
            ? f(UnsafeValue)
            : Result<TResult>.Error(UnsafeError);
    }
    
    public TResult Match<TResult>(
        Func<T, TResult> success,
        Func<Exception, TResult> error)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (error is null) throw new ArgumentNullException(nameof(error));
        return IsSuccess
            ? success(UnsafeValue)
            : error(UnsafeError);
    }
    
    public T GetOrElse(Func<Exception, T> fallback)
    {
        if (fallback is null) throw new ArgumentNullException(nameof(fallback));
        return IsSuccess
            ? UnsafeValue
            : fallback(UnsafeError);
    }
    
    public T GetOrElse(T fallbackValue) =>
        IsSuccess ? UnsafeValue : fallbackValue;
    
    public bool TryGetValue(out T value)
    {
        if (IsSuccess)
        {
            value = UnsafeValue;
            return true;
        }
        value = default!;
        return false;
    }

    public bool TryGetError(out Exception error)
    {
        if (IsError)
        {
            error = UnsafeError;
            return true;
        }
        error = null!;
        return false;
    }
    
    public Option<T> ToOption() =>
        IsSuccess ? Option<T>.Some(UnsafeValue) : Option<T>.None();
}