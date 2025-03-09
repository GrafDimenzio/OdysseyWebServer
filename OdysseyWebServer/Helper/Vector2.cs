using System.Diagnostics.CodeAnalysis;

namespace OdysseyWebServer.Helper;

public struct Vector2
{
    public double X { get; set; }
    public double Y { get; set; }
        
    public override string ToString() => $"({X:F0}, {Y:F0})";

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not Vector2 vec) return false;
        if (Math.Abs(X - vec.X) > 0.0001f) return false;
        if (Math.Abs(Y - vec.Y) > 0.0001f) return false;
        return true;
    }
    
    public override int GetHashCode() => HashCode.Combine(X, Y);
}