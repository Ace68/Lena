namespace Lena.Core;

public partial struct Option<T>
{
    public Option<TResult> Select<TResult>(Func<T, TResult> f)
        => Map(f);

    public Option<TResult> SelectMany<TResult>(
        Func<T, Option<TResult>> bind)
        => Bind(bind);

    public Option<TResult> SelectMany<TIntermediate, TResult>(
        Func<T, Option<TIntermediate>> bind,
        Func<T, TIntermediate, TResult> project)
    {
        return Bind(t =>
            bind(t).Map(u => project(t, u)));
    }
}
