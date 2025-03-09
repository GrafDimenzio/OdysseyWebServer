namespace OdysseyWebServer.Helper;

public struct Kingdom
{
    public string KingdomName { get; set; }
    public string MainStageName { get; set; }
    public double Rotation { get; set; }
    public Vector2 Offset { get; set; }
    public float Scale { get; set; }
    public List<SubArea> SubAreas { get; set; }
}