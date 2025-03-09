using Newtonsoft.Json.Linq;

namespace OdysseyWebServer;

public static class Configuration
{
    public static string Token { get; set; } = "";
    public static string Server { get; set; } = "";
    public static int Port { get; set; }
    public static int UpdateFrequency { get; set; }
    public static int UpdateFlippedPlayersFrequency { get; set; }
    
    public static bool OnlyRequestWhenNecessary { get; set; } = true;
    
    public static string ServerName { get; set; } = "";

    public static string UserName { get; set; } = "";
    public static string Password { get; set; } = "";

    public static void LoadConfig(ConfigurationManager manager)
    {
        var settingsPath = File.Exists("/.dockerenv") ? "/data/settings.json" : null;
        
        if (!string.IsNullOrWhiteSpace(settingsPath))
        {
            if (!File.Exists(settingsPath))
            {
                GenerateSettingsFile(settingsPath);
                Console.WriteLine($"Settings was generated in {settingsPath}.\nPlease configure a proper server ip/domain and restart the application.");
                while (true)
                {
                    Thread.Sleep(10000);
                }
                return;
            }
            var json = File.ReadAllText(settingsPath);
            var jObject = JObject.Parse(json);
            
            Token = jObject["token"]?.ToString() ?? "";
            Server = jObject["server"]?.ToString() ?? "";
            
            Port = ParseInt(jObject["port"]?.ToString(), 1027);
            UpdateFrequency = ParseInt(jObject["updateFrequency"]?.ToString(), 100);
            UpdateFlippedPlayersFrequency = ParseInt(jObject["updateFlippedPlayers"]?.ToString(), -1);
            OnlyRequestWhenNecessary = ParseBool(jObject["onlyRequestWhenNecessary"]?.ToString(), true);
            
            ServerName = jObject["serverName"]?.ToString() ?? "";
            UserName = manager["accountName"] ?? "admin";
            Password = manager["accountPassword"] ?? "admin";
            return;
        }
        
        Token = manager["Token"] ?? "";
        Server = manager["Server"] ?? "";
        Port = ParseInt(manager["Port"], 1027);
        UpdateFrequency = ParseInt(manager["UpdateFrequency"], 100);
        UpdateFlippedPlayersFrequency = ParseInt(manager["UpdateFlippedPlayersFrequency"], -1);
        OnlyRequestWhenNecessary = ParseBool(manager["OnlyRequestWhenNecessary"], true);
        ServerName = manager["ServerName"] ?? "Odyssey Online";
        UserName = manager["AccountName"] ?? "admin";
        Password = manager["AccountPassword"] ?? "admin";
    }

    private static int ParseInt(string? input, int defaultValue)
    {
        return int.TryParse(input, out var result) ? result : defaultValue;
    }

    private static bool ParseBool(string? input, bool defaultValue)
    {
        return bool.TryParse(input, out var result) ? result : defaultValue;
    }

    private static void GenerateSettingsFile(string file)
    {
        var jObject = new JObject()
        {
            ["token"] = "SecureToken",
            ["server"] = "",
            ["port"] = 1027,
            ["updateFrequency"] = 100,
            ["updateFlippedPlayers"] = -1,
            ["onlyRequestWhenNecessary"] = true,
            ["serverName"] = "Odyssey Online",
            ["accountName"] = "admin",
            ["accountPassword"] = "admin",
        };
        File.Create(file).Close();
        File.WriteAllText(file, jObject.ToString());
    }
}