namespace OdysseyWebServer.Helper;

public class Player
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public int GameMode { get; set; }
    public string Kingdom { get; set; }
    public string Stage { get; set; }
    public int Scenario { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public bool? Tagged { get; set; }
    public CostumeInfo Costume { get; set; }
    public string Capture { get; set; }
    public bool Is2D  { get; set; }
    public string IPv4 { get; set; }

    public override string ToString()
    {
        return $"{ID}:{Name} Mode:{GameMode} {Kingdom}:{Stage}:{Scenario} {Position} {Rotation} Tagged:{Tagged} {Costume} Capture:{Capture} 2D:{Is2D} {IPv4}";
    }
}