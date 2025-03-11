# OdysseyWebServer
The OdysseyWebServer is an application that hosts a web interface to communicate with the [SmoOnlineServer](https://github.com/Sanae6/SmoOnlineServer) through its JSON API.
This application provides a user-friendly UI to manage your server instead of using the command line.

- Ban List: Manage bans for IPs, IDs, and stages.
- Stage Ban: Easily ban any stage in the game via a UI element that displays all available stages.
- Configuration Options: Adjust settings such as POV, scenario merging, and max player count.
- Player Management: Send all or selected players to any stage in the game.
- Player List: View player details, including their costume and current capture, with images.
- Player View: Access all player statistics known to the server.
- Console Access: Manually execute server commands.
- Live Map: View real-time player positions for each kingdom.

## Setup
Currently, there are no pre-built releases available. The application is provided as a Docker image. It is recommended to use [docker-compose](https://github.com/GrafDimenzio/OdysseyWebServer/blob/master/docker-compose.yml) for hosting.

## Configuration
If running the application normally, you can configure it via appsettings.json.
However, when using a container, it is recommended to configure it through /data/settings.json instead.
You can also use environment variables, which will override settings in the JSON file.

```json
{
  "token": "SecureToken",
  "server": "ServerIP",
  "port": 1027,
  "updateFrequency": 100,
  "updateFlippedPlayers": -1,
  "onlyRequestWhenNecessary": true,
  "serverName": "Dimenzio's Odyssey Online",
  "accountName": "admin",
  "accountPassword": "admin"
}
```

## Configuration Options

- token: Authentication token for communication with the SMO server.
- server: The hostname/IP/domain of the server.
- port: The server port.
- updateFrequency: Interval (in milliseconds) between player list updates. Lower values increase network traffic but provide faster live map updates.
- updateFlippedPlayers: Leave this at -1 unless hosting multiple instances that flip players and require frequent updates.
- onlyRequestWhenNecessary: Disables requests when no one is using the application.
- serverName: The name displayed in the top left corner.
- accountName: The login username (required to restrict access due to player IP visibility).
- accountPassword: The login password.

## Configure SMOOnlineServer
Regardless of how you host the main server, you need to modify its settings file to enable JSON API support, as it is disabled by default. Your configuration should look like this:
```json
"JsonApi": {
    "Enabled": true,
    "Tokens": {
      "SECURETOKEN": [
        "Commands",
        "Commands/ban",
        "Commands/crash",
        "Commands/flip",
        "Commands/list",
        "Commands/loadsettings",
        "Commands/maxplayers",
        "Commands/rejoin",
        "Commands/restartserver",
        "Commands/scenario",
        "Commands/send",
        "Commands/sendall",
        "Commands/shine",
        "Commands/tag",
        "Commands/unban",
        "Status/Players",
        "Status/Players/Capture",
        "Status/Players/Costume",
        "Status/Players/GameMode",
        "Status/Players/ID",
        "Status/Players/IPv4",
        "Status/Players/Is2D",
        "Status/Players/Kingdom",
        "Status/Players/Name",
        "Status/Players/Position",
        "Status/Players/Rotation",
        "Status/Players/Scenario",
        "Status/Players/Stage",
        "Status/Players/Tagged"
      ]
    }
  }
```