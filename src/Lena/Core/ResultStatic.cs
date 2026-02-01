namespace Lena.Core;

/// <summary>
/// Static helper methods for creating Result instances.
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a Success result from a value. The type is inferred from the parameter.
    /// </summary>
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    
    /// <summary>
    /// Creates an Error result from an exception. The type must be specified at the call site.
    /// </summary>
    public static Result<T> Error<T>(Exception error) => Result<T>.Error(error);
    
    /// <summary>
    /// Creates an Error result from an error message. The type must be specified at the call site.
    /// </summary>
    public static Result<T> Error<T>(string message) => Result<T>.Error(message);
    
    /// <summary>
    /// Executes a function safely, catching any exceptions and converting them to Error results.
    /// </summary>
    public static Result<T> Try<T>(Func<T> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        
        try
        {
            return Success(f());
        }
        catch (Exception ex)
        {
            return Error<T>(ex);
        }
    }
    
    /// <summary>
    /// Executes an action safely, catching any exceptions and converting them to Error results.
    /// Returns Unit on success.
    /// </summary>
    public static Result<Unit> Try(Action action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));
        
        return Try(() =>
        {
            action();
            return Unit.Value;
        });
    }
}
