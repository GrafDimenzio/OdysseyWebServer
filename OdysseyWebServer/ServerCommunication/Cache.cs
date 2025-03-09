using System.Collections.Concurrent;
using OdysseyWebServer.Helper;

namespace OdysseyWebServer.ServerCommunication;

public static class Cache
{
    //I Store the Timers mostly for debug purposes. When Hot Reloading while debugging the timers would be disposed if not stored like this
    private static Timer _updatePlayerTimer;
    private static Timer _updateFlippedPlayerTimer;
    
    public static List<Player> Players = new();
    public static List<string> FlippedPlayers = [];
    public static ConcurrentDictionary<int, List<string>> BanList = new();
    private static readonly ConcurrentDictionary<Guid, Vector2> Cached2DPositions = new();

    public static event Action<List<Player>>? OnUpdatePlayers;
    public static event Action<List<string>>? OnUpdateFlippedPlayers;
    
    public static List<string> Permissions { get; private set; } = new();

    public static async Task Initialize()
    {
        Permissions = await RequestHandler.GetPermissions();
        await RefreshBans();
        _updatePlayerTimer = new Timer(_ => UpdatePlayers(), null, 0, Configuration.UpdateFrequency);
        UpdateFlippedPlayers(true);
        if (Configuration.UpdateFlippedPlayersFrequency > 0)
            _updateFlippedPlayerTimer = new Timer(_ => UpdateFlippedPlayers(), null, 0,
                Configuration.UpdateFlippedPlayersFrequency);
    }
    
    public static async Task RefreshBans()
    {
        BanList = new(await RequestHandler.GetBanList());
    }
    
    public static async Task UnBan(BanType type, string content)
    {
        await RequestHandler.ExecuteCommand("unban " + type.ToString().ToLower() + " " + content);
        await RefreshBans();
    }

    public static async Task<string> BanPlayer(string id)
    {
        var response = await RequestHandler.ExecuteCommand("ban player " + id);
        await RefreshBans();
        return string.Join("\n", response);
    }
    
    public static async Task<string> BanStage(string name)
    {
        var response = await RequestHandler.ExecuteCommand("ban stage " + name);
        await RefreshBans();
        return string.Join("\n", response);
    }

    public static async Task<List<string>> SetPlayerFlipState(string playerId, bool flipped)
    {
        var response = await RequestHandler.SetPlayerFlipState(playerId, flipped);
        lock (FlippedPlayers)
        {
            FlippedPlayers.Remove(playerId);
            if(flipped)
            {
                FlippedPlayers.Add(playerId);
            }
            OnUpdateFlippedPlayers?.Invoke([..FlippedPlayers]);
        }
        return response;
    }

    public static Vector2 GetMapPosition(Player player)
    {
        if (Cached2DPositions.TryGetValue(player.ID, out var pos))
        {
            return pos;
        }

        pos = Utils.GetMapPosition(player.Position, player.Kingdom, player.Stage);
        Cached2DPositions[player.ID] = pos;
        return pos;
    }

    private static async void UpdatePlayers()
    {
        try
        {
            if (OnUpdatePlayers == null && Configuration.OnlyRequestWhenNecessary)
            {
                Players.Clear();
                return;
            }

            var players = await RequestHandler.GetPlayers();
            Cached2DPositions.Clear();
            Players = players.ToList();
            OnUpdatePlayers?.Invoke([..players]);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static async void UpdateFlippedPlayers(bool force = false)
    {
        if (!force && OnUpdateFlippedPlayers == null && Configuration.OnlyRequestWhenNecessary)
        {
            return;
        }

        var flipped = await RequestHandler.GetFlippedPlayers();
        lock (FlippedPlayers)
        {
            FlippedPlayers = flipped.ToList();
        }
        OnUpdateFlippedPlayers?.Invoke(flipped.ToList());
    }
}