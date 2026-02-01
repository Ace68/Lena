namespace Lena.Core;

/// <summary>
/// Static helper methods for creating Option instances without specifying the type parameter.
/// </summary>
public static class Option
{
    /// <summary>
    /// Creates a Some option from a value. The type is inferred from the parameter.
    /// </summary>
    public static Option<T> Some<T>(T value) => Option<T>.Some(value);
    
    /// <summary>
    /// Creates a None option. The type must be specified at the call site.
    /// </summary>
    public static Option<T> None<T>() => Option<T>.None();
    
    /// <summary>
    /// Creates an Option from a nullable value. Returns None if null, Some otherwise.
    /// </summary>
    public static Option<T> FromNullable<T>(T? value) where T : class =>
        value is null ? None<T>() : Some(value);
        
    /// <summary>
    /// Creates an Option from a nullable value type. Returns None if null, Some otherwise.
    /// </summary>
    public static Option<T> FromNullable<T>(T? value) where T : struct =>
        value.HasValue ? Some(value.Value) : None<T>();
}
