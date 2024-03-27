using Newtonsoft.Json;

namespace BtcTurkApiCore.Models.WebSocket.Responses;

public class TradingView : BaseMessage
{
    [JsonProperty("C")]
    public string Close { get; set; }
    
    [JsonProperty("H")]
    public string High { get; set; }
    
    [JsonProperty("L")]
    public string Low { get; set; }
    
    [JsonProperty("O")]
    public string Open { get; set; }
    
    [JsonProperty("D")]
    public long Timestamp { get; set; }
    
    [JsonProperty("R")]
    public int Resolution { get; set; }
    
    [JsonProperty("V")]
    public string Volume { get; set; }
    
    [JsonProperty("P")]
    public string PairSymbol { get; set; }
}