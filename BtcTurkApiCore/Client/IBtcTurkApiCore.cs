using BtcTurkApiCore.Models.Api.Requests;
using BtcTurkApiCore.Models.Api.Responses;

namespace BtcTurkApiCore.Client;

public interface IBtcTurkApiCore
{
    Task<ExchangeInfo> GetExchangeInfoAsync();

    Task<Ticker> GetTickersAsync();

    Task<OrderBook> GetOrderBookAsync(string pairSymbol, int? limit);

    Task<Trade> GetTradesAsync(string pairSymbol);

    Task<OpenOrder> GetOpenOrdersAsync(string pairSymbol);

    Task<UserTransactionTrade> GetUserTransactionTradesAsync(long? orderId);
    
    Task<UserTransactionTrade> GetUserTransactionTradesAsync(string pairSymbol, long? startDate, long? endDate);
    
    Task<UserBalance> GetBalancesAsync();
    
    Task<CreateOrderResponse> CreateOrderRequestAsync(CreateOrderRequest request);

    Task<CancelOrderResponse?> CancelOrderRequestAsync(string id);
    
    Task<List<Ohlc>> GetOhlc(QueryOhlcRequest request);

    Task<Kline> GetKlines(QueryKlineRequest request);
}