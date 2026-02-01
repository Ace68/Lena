namespace Lena.Core;

/// <summary>
/// Represents a type with only one value, similar to void but usable in generic contexts.
/// Used in functional programming to represent "no meaningful value".
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    public static readonly Unit Value = new();
    
    public static Unit Default => Value;
    
    public bool Equals(Unit other) => true;
    
    public override bool Equals(object? obj) => obj is Unit;
    
    public override int GetHashCode() => 0;
    
    public override string ToString() => "()";
    
    public static bool operator ==(Unit left, Unit right) => true;
    
    public static bool operator !=(Unit left, Unit right) => false;
    
    public static implicit operator Unit(ValueTuple _) => Value;
}