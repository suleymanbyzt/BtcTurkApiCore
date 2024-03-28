using BtcTurkApiCore.Constants;
using BtcTurkApiCore.Models.WebSocket.Responses;

namespace BtcTurkApiCore.Websocket.Interfaces;

/// <summary>
/// BtcTurk Public WebSocket Client Interface
/// </summary>
public interface IBtcTurkPublicWebSocketClient
{
    /// <summary>
    /// Event called when TickerAll message received from WebSocket.
    /// </summary>
    event Action<TickerAll> OnTickerAll;

    /// <summary>
    /// Event called when TickerPair message received from WebSocket.
    /// </summary>
    event Action<TickerPair> OnTickerPair;

    /// <summary>
    /// Event called when OrderBook message received from WebSocket.
    /// </summary>
    event Action<OrderBookFull> OnOrderBook;

    /// <summary>
    /// Event called when TradeSingle message received from WebSocket.
    /// </summary>
    event Action<TradeSingle> OnTradeSingle;

    /// <summary>
    /// Event called when TradingView message received from WebSocket.
    /// </summary>
    event Action<TradingView> OnTradingView;
    
    /// <summary>
    /// Event called when an error occurs during WebSocket communication.
    /// </summary>
    event Action<Exception> OnError;

    /// <summary>
    /// Starts the WebSocket client.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task StartSocketClientAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops the WebSocket client.
    /// </summary>
    Task StopSocketClientAsync();

    /// <summary>
    /// Restarts the WebSocket client.
    /// </summary>
    Task RestartSocketClientAsync();

    /// <summary>
    /// Sends a subscription request to the WebSocket server.
    /// </summary>
    /// <param name="channel">Channel</param>
    /// <param name="event">Event (e.g., 'BTCTRY, ETHUSDT, all')</param>
    Task SendSubscriptionRequest(Channels channel, string @event);
}