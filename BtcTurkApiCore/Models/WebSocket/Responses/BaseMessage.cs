using Newtonsoft.Json;

namespace BtcTurkApiCore.Models.WebSocket.Responses;

public class BaseMessage
{
    [JsonProperty("channel")]
    public string Channel { get; set; }

    [JsonProperty("event")]
    public string Event { get; set; }

    [JsonProperty("type")]
    public int Type { get; set; }
}