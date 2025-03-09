namespace OdysseyWebServer.Helper;

public struct Quaternion
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }
        
    public override string ToString() => $"({X:F5}, {Y:F5}, {Z:F5}, {W:F5})";
}