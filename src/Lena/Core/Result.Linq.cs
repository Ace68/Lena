namespace Lena.Core;

public readonly partial struct Result<T>
{
    public Result<TResult> Select<TResult>(Func<T, TResult> f) => Map(f);
    
    public Result<TResult> SelectMany<TResult>(Func<T, Result<TResult>> f) => Bind(f);
    
    public Result<TResult> SelectMany<TIntermediate, TResult>(
        Func<T, Result<TIntermediate>> f,
        Func<T, TIntermediate, TResult> resultSelector)
    {
        if (f is null) throw new ArgumentNullException(nameof(f));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
        
        return Bind(value =>
            f(value).Bind(intermediate =>
                Result<TResult>.Success(resultSelector(value, intermediate))));
    }
}
