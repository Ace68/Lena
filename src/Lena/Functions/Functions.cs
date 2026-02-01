namespace Lena.Functions;

/// <summary>
/// Common higher-order functions for functional programming.
/// </summary>
public static class Functions
{
    /// <summary>
    /// Identity function - returns the input unchanged.
    /// </summary>
    public static T Identity<T>(T value) => value;
    
    /// <summary>
    /// Constant function - returns a function that always returns the same value.
    /// </summary>
    public static Func<T, TResult> Constant<T, TResult>(TResult value) => _ => value;
    
    /// <summary>
    /// Composes two functions: f(g(x)).
    /// </summary>
    public static Func<T, TResult> Compose<T, TIntermediate, TResult>(
        Func<TIntermediate, TResult> f,
        Func<T, TIntermediate> g) =>
        x => f(g(x));
    
    /// <summary>
    /// Curries a two-parameter function into nested single-parameter functions.
    /// </summary>
    public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(
        Func<T1, T2, TResult> f) =>
        x => y => f(x, y);
    
    /// <summary>
    /// Uncurries nested single-parameter functions into a two-parameter function.
    /// </summary>
    public static Func<T1, T2, TResult> Uncurry<T1, T2, TResult>(
        Func<T1, Func<T2, TResult>> f) =>
        (x, y) => f(x)(y);
    
    /// <summary>
    /// Partial application - fixes the first parameter of a two-parameter function.
    /// </summary>
    public static Func<T2, TResult> Partial<T1, T2, TResult>(
        Func<T1, T2, TResult> f,
        T1 value) =>
        y => f(value, y);
    
    /// <summary>
    /// Flips the order of parameters in a two-parameter function.
    /// </summary>
    public static Func<T2, T1, TResult> Flip<T1, T2, TResult>(
        Func<T1, T2, TResult> f) =>
        (y, x) => f(x, y);
}
