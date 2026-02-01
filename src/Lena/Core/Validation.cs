﻿namespace Lena.Core;

/// <summary>
/// Represents a validation result that can accumulate multiple errors.
/// Unlike Result which fails fast, Validation continues collecting errors.
/// </summary>
/// <typeparam name="T">The type of the success value</typeparam>
public readonly struct Validation<T>
{
    private readonly T _value;
    private readonly IReadOnlyList<string> _errors;
    
    public bool IsValid { get; }
    public bool IsInvalid => !IsValid;
    public IReadOnlyList<string> Errors => _errors ?? Array.Empty<string>();
    
    private T UnsafeValue => _value;
    
    private Validation(T value, IReadOnlyList<string> errors, bool isValid)
    {
        _value = value;
        _errors = errors;
        IsValid = isValid;
    }
    
    public static Validation<T> Valid(T value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));
        return new Validation<T>(value, Array.Empty<string>(), true);
    }
    
    public static Validation<T> Invalid(params string[] errors)
    {
        if (errors is null || errors.Length == 0)
            throw new ArgumentException("At least one error is required", nameof(errors));
        return new Validation<T>(default!, errors.ToList(), false);
    }
    
    public static Validation<T> Invalid(IEnumerable<string> errors)
    {
        var errorList = errors?.ToList() ?? throw new ArgumentNullException(nameof(errors));
        if (errorList.Count == 0)
            throw new ArgumentException("At least one error is required", nameof(errors));
        return new Validation<T>(default!, errorList, false);
    }
    
    public Validation<TResult> Map<TResult>(Func<T, TResult> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        return IsValid
            ? Validation<TResult>.Valid(f(UnsafeValue))
            : Validation<TResult>.Invalid(Errors);
    }
    
    public Validation<TResult> Bind<TResult>(Func<T, Validation<TResult>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        
        if (IsInvalid)
            return Validation<TResult>.Invalid(Errors);
            
        var result = f(UnsafeValue);
        return result.IsValid
            ? result
            : Validation<TResult>.Invalid(Errors.Concat(result.Errors));
    }
    
    public TResult Match<TResult>(
        Func<T, TResult> valid,
        Func<IReadOnlyList<string>, TResult> invalid)
    {
        if (valid is null) throw new ArgumentNullException(nameof(valid));
        if (invalid is null) throw new ArgumentNullException(nameof(invalid));
        return IsValid
            ? valid(UnsafeValue)
            : invalid(Errors);
    }
    
    public T GetOrElse(Func<IReadOnlyList<string>, T> fallback)
    {
        if (fallback is null) throw new ArgumentNullException(nameof(fallback));
        return IsValid ? UnsafeValue : fallback(Errors);
    }
    
    public T GetOrElse(T fallbackValue) =>
        IsValid ? UnsafeValue : fallbackValue;
    
    public Result<T> ToResult() =>
        IsValid 
            ? Result<T>.Success(UnsafeValue) 
            : Result<T>.Error(string.Join("; ", Errors));
            
    public Option<T> ToOption() =>
        IsValid ? Option<T>.Some(UnsafeValue) : Option<T>.None();
    
    /// <summary>
    /// Combines two validations using applicative functor pattern.
    /// If both are valid, applies the function. If either has errors, combines all errors.
    /// </summary>
    public static Validation<TResult> Apply<T1, T2, TResult>(
        Validation<T1> validation1,
        Validation<T2> validation2,
        Func<T1, T2, TResult> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        
        var allErrors = new List<string>();
        
        if (validation1.IsInvalid)
            allErrors.AddRange(validation1.Errors);
            
        if (validation2.IsInvalid)
            allErrors.AddRange(validation2.Errors);
            
        if (allErrors.Any())
            return Validation<TResult>.Invalid(allErrors);
            
        return Validation<TResult>.Valid(f(validation1.UnsafeValue, validation2.UnsafeValue));
    }
}
