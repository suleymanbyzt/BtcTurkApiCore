namespace BtcTurkApiCore.Models.WebSocket.Responses;

public class OrderInserted
{
    public int Type { get; set; }

    public long Id { get; set; }

    public string Symbol { get; set; }
    
    public string Price { get; set; }

    public string Amount { get; set; }

    public string DenomLeft { get; set; }

    public string NumLeft { get; set; }

    public int OrderType { get; set; } //buy = 0, sell = 1
    
    public int OrderMethod { get; set; } //limit = 0, market = 1

    public int PairId { get; set; }

    public long UserId { get; set; }
    
    public string NewOrderClientId { get; set; }
}