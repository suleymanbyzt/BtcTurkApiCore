namespace BtcTurkApiCore.Models.Api.Requests;

public class QueryKlineRequest
{
    public required string PairSymbol { get; set; }

    public required int Resolution { get; set; }

    public required long From { get; set; }

    public required long To { get; set; }
}