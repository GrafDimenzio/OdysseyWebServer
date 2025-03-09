using System.Reflection;
using System.Text.Json;
using OdysseyWebServer.Helper;

namespace OdysseyWebServer.ServerCommunication;

public class Test
{
    public static void Send()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream("SMOO_Server_WebInterface.kingdoms.json");
        var reader = new StreamReader(stream!);
        var json = reader.ReadToEndAsync().GetAwaiter().GetResult();
        Console.WriteLine(json);
        var obj = JsonSerializer.Deserialize<List<Kingdom>>(json) ?? new();
        foreach (var kingdom in obj)
        {
            Console.WriteLine(kingdom.KingdomName+" " + kingdom.MainStageName);
            foreach (var subArea in kingdom.SubAreas)
            {
                Console.WriteLine(subArea.SubAreaName);
            }
        }
        /*
        var perm = RequestHandler.GetPermissions().GetAwaiter().GetResult();
        foreach (var permission in perm)
        {
            //Console.WriteLine(permission);
        }

        //Console.WriteLine(RequestHandler.SendRequest(new Request(RequestType.Command) {RequestData = "list"}).GetAwaiter().GetResult()?.ToString());
        var players = RequestHandler.GetPlayers().GetAwaiter().GetResult();
        foreach (var player in players)
        {
            //Console.WriteLine(player.ToString());
        }

        RequestHandler.ExecuteCommand("flip add 5e25b5d5-8a47-2a26-7650-87469eda66fa").GetAwaiter().GetResult();
        var response = RequestHandler.GetFlippedPlayers().GetAwaiter().GetResult();
        foreach (var line in response)
        {
            Console.WriteLine(line);
        }
        */
    }
}