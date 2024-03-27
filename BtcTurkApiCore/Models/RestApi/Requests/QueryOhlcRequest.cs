namespace BtcTurkApiCore.Models.Api.Requests;

public class QueryOhlcRequest
{
    public required string PairSymbol { get; set; }

    public required long From { get; set; }

    public required long To { get; set; }
}