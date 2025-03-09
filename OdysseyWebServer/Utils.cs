using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
using OdysseyWebServer.Helper;

namespace OdysseyWebServer;

public static class Utils
{
    public static readonly Version Version = new(0,1,0);
    
    static Utils()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream("OdysseyWebServer.kingdoms.json");
        var reader = new StreamReader(stream!);
        var json = reader.ReadToEndAsync().GetAwaiter().GetResult();
        Kingdoms = JsonSerializer.Deserialize<List<Kingdom>>(json) ?? [];
    }

    public static readonly List<Kingdom> Kingdoms;

    public static ReadOnlyDictionary<string, string> TeleportNames =
        new(new Dictionary<string, string>()
        {
            {"Cap Kingdom", "cap"},
            {"Cascade Kingdom", "cascade"},
            {"Sand Kingdom", "sand"},
            {"Lake Kingdom", "lake"},
            {"Wooded Kingdom", "wooded"},
            {"Cloud Kingdom", "cloud"},
            {"Lost Kingdom", "lost"},
            {"Metro Kingdom", "metro"},
            {"Snow Kingdom", "snow"},
            {"Seaside Kingdom", "sea"},
            {"Luncheon Kingdom", "lunch"},
            {"Ruined Kingdom", "ruined"},
            {"Bowser's Kingdom", "bowser"},
            {"Moon Kingdom", "moon"},
            {"Mushroom Kingdom", "mush"},
            {"Dark Side", "dark"},
            {"Darker Side", "darker"},
            {"Odyssey", "odyssey"},
        });
    
    public static Vector2 GetMapPosition(Vector3 position, string kingdomName, string stageName = "")
    {
        if (Kingdoms.All(x => x.KingdomName != kingdomName))
            return new Vector2() {X = position.X, Y = position.Z};
        
        var kingdom = Kingdoms.FirstOrDefault(x => x.KingdomName == kingdomName);
        var pos = new Vector2();
        if (kingdom.MainStageName == stageName)
        {
            pos.X = position.X;
            pos.Y = position.Z;
        }
        else if(kingdom.SubAreas.Any(x => x.SubAreaName == stageName))
        {
            var subArea = kingdom.SubAreas.First(x => x.SubAreaName == stageName);
            pos = subArea.Position;
        }
        else
        {
            return new Vector2 {X = 2048, Y = 2048};
        }
        if (kingdom.Rotation != 0)
        {
            var angle = Math.PI * -kingdom.Rotation;
            var oldY = pos.Y;
            var oldX = pos.X;
            
            pos.Y = oldY * Math.Cos(angle) - oldX * Math.Sin(angle);
            pos.X = oldY * Math.Sin(angle) + oldX * Math.Cos(angle);
        }

        pos.X += kingdom.Offset.X;
        pos.Y += kingdom.Offset.Y;

        pos.X /= kingdom.Scale;
        pos.Y /= kingdom.Scale;
        
        return pos;
    }
}