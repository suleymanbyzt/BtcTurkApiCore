using Newtonsoft.Json;

namespace BtcTurkApiCore.Models.WebSocket.Responses;

public class TickerPair
{
    [JsonProperty("B")]
    public string BidPrice { get; set; }

    [JsonProperty("A")]
    public string AskPrice { get; set; }

    [JsonProperty("BA")]
    public string BidAmount { get; set; }

    [JsonProperty("AA")]
    public string AskAmount { get; set; }

    [JsonProperty("PS")]
    public string PairSymbol { get; set; }

    [JsonProperty("O")]
    public string Open { get; set; }

    [JsonProperty("H")]
    public string High { get; set; }

    [JsonProperty("L")]
    public string Low { get; set; }

    [JsonProperty("DP")]
    public string DailyPercent { get; set; }

    [JsonProperty("LA")]
    public string LastPrice { get; set; }

    [JsonProperty("V")]
    public string Volume { get; set; }

    [JsonProperty("AV")]
    public string Average { get; set; }

    [JsonProperty("D")]
    public string DailyChange { get; set; }

    [JsonProperty("DS")]
    public string DenominatorSymbol { get; set; }

    [JsonProperty("NS")]
    public string NumeratorSymbol { get; set; }

    [JsonProperty("PId")]
    public int PairId { get; set; }
}