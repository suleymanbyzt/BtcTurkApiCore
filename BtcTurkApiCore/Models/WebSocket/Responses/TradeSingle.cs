using BtcTurkApiCore.Models.Api.Enums;
using Newtonsoft.Json;

namespace BtcTurkApiCore.Models.WebSocket.Responses;

public class TradeSingle : BaseMessage
{
    [JsonProperty("A")]
    public string Amount { get; set; }

    [JsonProperty("P")]
    public string Price { get; set; }

    [JsonProperty("I")]
    public string Id { get; set; }

    [JsonProperty("D")]
    public string Timestamp { get; set; }

    [JsonProperty("PS")]
    public string PairSymbol { get; set; }

    [JsonProperty("S")]
    public OrderType OrderSide { get; set; }
}