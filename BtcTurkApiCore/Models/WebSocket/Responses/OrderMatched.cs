namespace BtcTurkApiCore.Models.WebSocket.Responses;

public class OrderMatched
{
    public int Type { get; set; }
    
    public long Id { get; set; }

    public int Method { get; set; }

    public int Pair { get; set; }

    public string Timestamp { get; set; }
    
    public string Symbol { get; set; }

    public bool IsBid { get; set; }

    public string Price { get; set; }

    public string Amount { get; set; }

    public string ClientId { get; set; }
}