using Lena.Core;

namespace Lena.Extensions;

/// <summary>
/// Extension methods for common functional programming patterns.
/// </summary>
public static class FunctionalExtensions
{
    /// <summary>
    /// Applies a side effect to a value and returns the value unchanged.
    /// Useful for debugging and logging in functional pipelines.
    /// </summary>
    public static T Tap<T>(this T value, Action<T> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));
        action(value);
        return value;
    }
    
    /// <summary>
    /// Converts a nullable value to an Option.
    /// </summary>
    public static Option<T> ToOption<T>(this T? value) where T : struct =>
        Option.FromNullable(value);
        
    /// <summary>
    /// Converts a nullable reference to an Option.
    /// </summary>
    public static Option<T> ToOption<T>(this T? value) where T : class =>
        Option.FromNullable(value);
    
    /// <summary>
    /// Pipes a value through a function. Allows for more readable function composition.
    /// </summary>
    public static TResult Pipe<T, TResult>(this T value, Func<T, TResult> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        return f(value);
    }
    
    /// <summary>
    /// Applies a predicate to a value and returns Some(value) if true, None if false.
    /// </summary>
    public static Option<T> Where<T>(this T value, Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        return predicate(value) ? Option.Some(value) : Option.None<T>();
    }
}

/// <summary>
/// Extension methods for working with sequences and Options.
/// </summary>
public static class SequenceExtensions
{
    /// <summary>
    /// Returns the first element as Some, or None if the sequence is empty.
    /// </summary>
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        
        foreach (var item in source)
        {
            return Option<T>.Some(item);
        }
        return Option<T>.None();
    }
    
    /// <summary>
    /// Returns the first element matching the predicate as Some, or None if no match found.
    /// </summary>
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        
        foreach (var item in source)
        {
            if (predicate(item))
                return Option<T>.Some(item);
        }
        return Option<T>.None();
    }
    
    /// <summary>
    /// Returns the last element as Some, or None if the sequence is empty.
    /// </summary>
    public static Option<T> LastOrNone<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        
        var found = false;
        var result = default(T)!;
        
        foreach (var item in source)
        {
            found = true;
            result = item;
        }
        
        return found ? Option<T>.Some(result) : Option<T>.None();
    }
    
    /// <summary>
    /// Filters and unwraps Some values from a sequence of Options.
    /// </summary>
    public static IEnumerable<T> ChooseSome<T>(this IEnumerable<Option<T>> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        
        return source
            .Where(opt => opt.IsSome)
            .Select(opt => opt.GetOrThrow());
    }
    
    /// <summary>
    /// Applies a function that returns an Option to each element and filters out None values.
    /// </summary>
    public static IEnumerable<TResult> Choose<T, TResult>(
        this IEnumerable<T> source,
        Func<T, Option<TResult>> chooser)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (chooser is null) throw new ArgumentNullException(nameof(chooser));
        
        return source
            .Select(chooser)
            .ChooseSome();
    }
    
    /// <summary>
    /// Traverses a sequence of values, applying a function that returns an Option to each,
    /// and returns Some of the results if all succeed, or None if any fail.
    /// </summary>
    public static Option<IEnumerable<TResult>> Traverse<T, TResult>(
        this IEnumerable<T> source,
        Func<T, Option<TResult>> f)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (f is null) throw new ArgumentNullException(nameof(f));
        
        var results = new List<TResult>();
        
        foreach (var item in source)
        {
            var result = f(item);
            if (result.IsNone)
                return Option<IEnumerable<TResult>>.None();
            results.Add(result.GetOrThrow());
        }
        
        return Option<IEnumerable<TResult>>.Some(results);
    }
    
    /// <summary>
    /// Sequences a collection of Options into an Option of collection.
    /// Returns Some if all Options are Some, None if any is None.
    /// </summary>
    public static Option<IEnumerable<T>> Sequence<T>(this IEnumerable<Option<T>> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        
        return source.Traverse(Functions.Functions.Identity);
    }
}
