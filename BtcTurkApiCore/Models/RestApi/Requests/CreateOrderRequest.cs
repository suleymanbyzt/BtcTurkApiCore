using BtcTurkApiCore.Models.Api.Enums;

namespace BtcTurkApiCore.Models.Api.Requests;

public class CreateOrderRequest
{
    public required string Quantity { get; set; }

    public string? Price { get; set; }

    public string? NewOrderClientId { get; set; }

    public required OrderMethod OrderMethod { get; set; }

    public required OrderType OrderType { get; set; }

    public required string PairSymbol { get; set; }
}