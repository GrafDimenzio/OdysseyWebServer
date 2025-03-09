using Newtonsoft.Json.Linq;

namespace OdysseyWebServer.ServerCommunication;

public struct Request(RequestType type)
{
    public RequestType RequestType { get; set; } = type;
    public string RequestData { get; set; } = "";

    public override string ToString()
    {
        var requestJObject = new JObject()
        {
            ["Token"] = Configuration.Token,
            ["Type"] = RequestType.ToString(),
        };

        if (RequestType == RequestType.Command)
        {
            requestJObject["Data"] = RequestData;
        }

        return "{\"API_JSON_REQUEST\": " + requestJObject + "}\n";
    }
}