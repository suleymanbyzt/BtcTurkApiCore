namespace BtcTurkApiCore.Models.WebSocket.Responses;

public class UserTrade
{
    public int Type { get; set; } = 423;

    public long Id { get; set; }

    public long OrderId { get; set; }

    public long Timestamp { get; set; }

    public string NumeratorSymbol { get; set; }

    public string DenominatorSymbol { get; set; }

    public string Amount { get; set; }

    public string Fee { get; set; }

    public string Tax { get; set; }

    public string Price { get; set; }

    public string OrderType { get; set; }

    public string OrderClientId { get; set; }

    public long PreciseAmount { get; set; }
}