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

    public static readonly IReadOnlyList<ConfigSetter> Configs = new List<ConfigSetter>()
    {
        new("token", nameof(Token), null, ""),
        new("server", nameof(Server), null, ""),
        new("port", nameof(Port), ParseInt, 1027),

        new("updateFrequency", nameof(UpdateFrequency), ParseInt, 100),
        new("updateFlippedPlayers", nameof(UpdateFlippedPlayersFrequency), ParseInt, -1),
        new("onlyRequestWhenNecessary", nameof(OnlyRequestWhenNecessary), ParseBool, true),

        new("serverName", nameof(ServerName), null, "Odyssey Server"),
        new("accountName", nameof(UserName), null, "admin"),
        new("accountPassword", nameof(Password), null, "admin"),
    };

    public static void LoadConfig(ConfigurationManager manager)
    {
        foreach (var config in Configs)
        {
            var value = manager[config.ConfigName];
            if (value == null) continue;
            config.SetConfig(value);
        }

        var settingsPath = GetSettingsPath();
        if (settingsPath != null)
        {
            if (!File.Exists(settingsPath))
            {
                GenerateSettingsFile(settingsPath);
            }
            var json = File.ReadAllText(settingsPath);
            var jObject = JObject.Parse(json);
            
            foreach (var config in Configs)
            {
                var value = jObject[config.ConfigName]?.ToString();
                if (value == null) continue;
                config.SetConfig(value);
            }
        }
        
        foreach (var config in Configs)
        {
            var value = Environment.GetEnvironmentVariable(config.ConfigName.ToUpper());
            if (value == null) continue;
            config.SetConfig(value);
        }

        if (!string.IsNullOrWhiteSpace(Server) && Port >= 0) return;
        
        Console.WriteLine($"You need to configure a proper Host/IP and Port. Please check your configuration and restart the application. Current Configuration:{Server}:{Port}");
        while (true)
        {
            Thread.Sleep(1000);
        }
    }

    private static object ParseInt(string input, object defaultValue)
    {
        return int.TryParse(input, out var result) ? result : defaultValue;
    }
    
    private static object ParseBool(string input, object defaultValue)
    {
        return bool.TryParse(input, out var result) ? result : defaultValue;
    }

    private static string? GetSettingsPath()
    {
        if (File.Exists("/.dockerenv"))
        {
            return "/data/settings.json";
        }
        
        return File.Exists("settings.json") ? "settings.json" : null;
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

    public class ConfigSetter(string configName, string propertyName, Func<string,object,object>? parser, object defaultValue)
    {
        public string ConfigName => configName;
        
        public void SetConfig(string value)
        {
            var finalValue = parser?.Invoke(value, defaultValue) ?? value;
            typeof(Configuration).GetProperty(propertyName)?.SetValue(this, finalValue);
        }
    }
}