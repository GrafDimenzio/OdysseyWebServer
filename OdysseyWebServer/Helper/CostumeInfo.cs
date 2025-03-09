namespace OdysseyWebServer.Helper;

public struct CostumeInfo
{
    public string Cap { get; set; }
    public string Body { get; set; }
        
    public override string ToString() => $"({Cap}, {Body})";
}