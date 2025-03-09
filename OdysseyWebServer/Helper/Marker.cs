using OdysseyWebServer.ServerCommunication;

namespace OdysseyWebServer.Helper;

public struct Marker
{
    public string Name { get; set; }
    public Guid ID { get; set; }
    public Vector2 Position { get; set; }

    public static Marker FromPlayer(Player player) => new Marker()
    {
        Name = player.Name,
        ID = player.ID,
        Position = Cache.GetMapPosition(player),
    };
}