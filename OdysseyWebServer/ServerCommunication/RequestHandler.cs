using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;
using OdysseyWebServer.Helper;

namespace OdysseyWebServer.ServerCommunication;

public static class RequestHandler
{
    public static async Task<List<Player>> GetPlayers()
    {
        var response = await SendRequest(new Request(RequestType.Status));
        var list = response?["Players"]?.ToObject<List<Player>>();
        return list ?? [];
    }

    public static async Task<Dictionary<int, List<string>>> GetBanList()
    {
        var response = await ExecuteCommand("ban list");
        var i = 0;
        var result = new Dictionary<int, List<string>>()
        {
            {1, new List<string>()},
            {2, new List<string>()},
            {3, new List<string>()}
        };
        foreach (var line in response)
        {
            switch (line)
            {
                case "Banned IPv4 addresses:":
                    i = 1;
                    break;
                
                case "Banned profile IDs:":
                    i = 2;
                    break;
                case "Banned stages:":
                    i = 3;
                    break;
                default:
                    if (i is < 1 or > 3)
                        continue;

                    result[i].Add(line[2..]);
                    break;
            }
        }
        return result;
    }

    public static async Task<List<string>> ExecuteCommand(string command)
    {
        var response = await SendRequest(new Request(RequestType.Command) {RequestData = command});
        var list = response?["Output"]?.ToObject<List<string>>();
        return list ?? [];
    }
    
    public static async Task<List<string>> GetPermissions()
    {
        var response = await SendRequest(new Request(RequestType.Permissions));
        var list = response?["Permissions"]?.ToObject<List<string>>();
        return list ?? [];
    }

    public static async Task<string[]> GetFlippedPlayers()
    {
        var response = await ExecuteCommand("flip list");
        return response.Count == 0 ? [] : response[0].Replace("User ids: ", "").Split(", ");
    }

    public static async Task<List<string>> SetPlayerFlipState(string playerId, bool flipped)
    {
        return await ExecuteCommand("flip " + (flipped ? "add" : "remove") + " " + playerId);
    }
    
    public static async Task<JObject?> SendRequest(Request request)
    {
        try
        {
            //TODO: Fix this to just use a single Connection and not create a new one on every Request
            //For some Reason the SMOO Server had Problems when I tried to send multiple request through the same connections
            var client = new TcpClient(Configuration.Server, Configuration.Port);
            var stream = client.GetStream();
            
            var bytes = Encoding.UTF8.GetBytes(request.ToString());
            await stream.WriteAsync(bytes);
            
            var buffer = new byte[1024];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return JObject.Parse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}