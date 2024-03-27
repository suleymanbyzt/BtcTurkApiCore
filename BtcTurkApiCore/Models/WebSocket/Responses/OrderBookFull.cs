using Newtonsoft.Json;

namespace BtcTurkApiCore.Models.WebSocket.Responses;

public class OrderBookFull : BaseMessage
{
    [JsonProperty("AO")]
    public IEnumerable<ObItems> AskOrders { get; set; }
    
    [JsonProperty("BO")]
    public IEnumerable<ObItems> BidOrders { get; set; }

    [JsonProperty("CS")]
    public long ChangeSet { get; set; }

    [JsonProperty("PS")]
    public string PairSymbol { get; set; }
}


public class ObItems
{
    [JsonProperty("A")]
    public string Amount { get; set; }

    [JsonProperty("P")]
    public string Price { get; set; }
}