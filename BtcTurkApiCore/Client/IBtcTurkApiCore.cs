using BtcTurkApiCore.Models.Api.Requests;
using BtcTurkApiCore.Models.Api.Responses;

namespace BtcTurkApiCore.Client;

/// <summary>
/// Provides an interface to interact with BtcTurk API, offering methods to access trading data and execute trading operations.
/// </summary>
public interface IBtcTurkApiCore
{
    /// <summary>
    /// Get exchange information including trading pairs and their limits.
    /// </summary>
    Task<ExchangeInfo> GetExchangeInfoAsync();

    /// <summary>
    /// Gets the current tickers for all pairs.
    /// </summary>
    Task<Ticker> GetTickersAsync();

    /// <summary>
    /// Get the order book for a specific trading pair.
    /// </summary>
    /// <param name="pairSymbol">Trading pair symbol (e.g., 'BTCTRY').</param>
    /// <param name="limit">Optional limit for number of orders to retrieve. Default value: 25</param>
    Task<OrderBook> GetOrderBookAsync(string pairSymbol, int? limit);

    /// <summary>
    /// Get last 50 trades for a specific trading pair.
    /// </summary>
    /// <param name="pairSymbol">Trading pair symbol (e.g., 'BTCTRY').</param>
    Task<Trade> GetTradesAsync(string pairSymbol);

    /// <summary>
    /// Get open orders for a specific trading pair.
    /// </summary>
    /// <param name="pairSymbol">Trading pair symbol (e.g., 'BTCTRY').</param>
    Task<OpenOrder> GetOpenOrdersAsync(string pairSymbol);

    /// <summary>
    /// Get user transactions for a specific trading pair.
    /// </summary>
    /// <param name="orderId">orderId of the order. (e.g., '12487321868').</param>
    Task<UserTransactionTrade> GetUserTransactionTradesAsync(long? orderId);
    
    /// <summary>
    /// Get user transactions for a specific trading pair.
    /// </summary>
    /// <param name="pairSymbol">Trading pair symbol (e.g., 'BTCTRY').</param>
    /// <param name="startDate">Start date of the transactions in Unix timestamp format.</param>
    /// <param name="endDate">End date of the transactions in Unix timestamp format.</param>
    Task<UserTransactionTrade> GetUserTransactionTradesAsync(string pairSymbol, long? startDate, long? endDate);
    
    /// <summary>
    /// Get user balance information.
    /// </summary>
    Task<UserBalance> GetBalancesAsync();
    
    /// <summary>
    /// Create a new order.
    /// </summary>
    /// <param name="request">CreateOrderRequest object containing order details.</param>
    Task<CreateOrderResponse> CreateOrderRequestAsync(CreateOrderRequest request);

    /// <summary>
    /// Cancel an existing order. Status must be 'OPEN' or 'PARTIAL'.
    /// </summary>
    /// <param name="id">Order id to cancel.</param>
    Task<CancelOrderResponse?> CancelOrderRequestAsync(string id);
    
    /// <summary>
    /// Get OHLC (Open, High, Low, Close) data for a specific trading pair.
    /// </summary>
    /// <param name="request">QueryOhlcRequest object containing query details.</param>
    Task<List<Ohlc>> GetOhlc(QueryOhlcRequest request);

    /// <summary>
    /// Get Kline data for a specific trading pair.
    /// </summary>
    /// <param name="request">QueryKlineRequest object containing query details.</param>
    Task<Kline> GetKlines(QueryKlineRequest request);
}