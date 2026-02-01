﻿namespace Lena.Core;

/// <summary>
/// Represents a value that can be one of two types: Left or Right.
/// By convention, Left is used for error/failure cases and Right for success cases.
/// </summary>
/// <typeparam name="TLeft">The type of the Left value</typeparam>
/// <typeparam name="TRight">The type of the Right value</typeparam>
public readonly struct Either<TLeft, TRight>
{
    private readonly TLeft _left;
    private readonly TRight _right;
    
    public bool IsLeft { get; }
    public bool IsRight => !IsLeft;
    
    private TLeft UnsafeLeft => _left;
    private TRight UnsafeRight => _right;
    
    private Either(TLeft left, TRight right, bool isLeft)
    {
        _left = left;
        _right = right;
        IsLeft = isLeft;
    }
    
    public static Either<TLeft, TRight> Left(TLeft value) =>
        new(value, default!, true);
        
    public static Either<TLeft, TRight> Right(TRight value) =>
        new(default!, value, false);
    
    public Either<TLeft, TResult> Map<TResult>(Func<TRight, TResult> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        return IsLeft
            ? Either<TLeft, TResult>.Left(UnsafeLeft)
            : Either<TLeft, TResult>.Right(f(UnsafeRight));
    }
    
    public Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        return IsLeft
            ? Either<TNewLeft, TRight>.Left(f(UnsafeLeft))
            : Either<TNewLeft, TRight>.Right(UnsafeRight);
    }
    
    public Either<TLeft, TResult> Bind<TResult>(Func<TRight, Either<TLeft, TResult>> f)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        return IsLeft
            ? Either<TLeft, TResult>.Left(UnsafeLeft)
            : f(UnsafeRight);
    }
    
    public TResult Match<TResult>(
        Func<TLeft, TResult> left,
        Func<TRight, TResult> right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        return IsLeft
            ? left(UnsafeLeft)
            : right(UnsafeRight);
    }
    
    public void Match(Action<TLeft> left, Action<TRight> right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        
        if (IsLeft)
            left(UnsafeLeft);
        else
            right(UnsafeRight);
    }
    
    public TRight GetRightOrElse(Func<TLeft, TRight> fallback)
    {
        if (fallback is null) throw new ArgumentNullException(nameof(fallback));
        return IsLeft ? fallback(UnsafeLeft) : UnsafeRight;
    }
    
    public TLeft GetLeftOrElse(Func<TRight, TLeft> fallback)
    {
        if (fallback is null) throw new ArgumentNullException(nameof(fallback));
        return IsRight ? fallback(UnsafeRight) : UnsafeLeft;
    }
    
    public Option<TRight> RightOption() =>
        IsRight ? Option<TRight>.Some(UnsafeRight) : Option<TRight>.None();
        
    public Option<TLeft> LeftOption() =>
        IsLeft ? Option<TLeft>.Some(UnsafeLeft) : Option<TLeft>.None();
}
