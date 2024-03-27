using BtcTurkApiCore.Models.Options;
using BtcTurkApiCore.Models.WebSocket.Responses;

namespace BtcTurkApiCore.Websocket.Interfaces;

/// <summary>
/// BtcTurk Private WebSocket Client Interface
/// </summary>
public interface IBtcTurkPrivateWebSocketClient
{
    /// <summary>
    /// Event called when OrderMatched message received from WebSocket EventId: 441.
    /// </summary>
    event Action<OrderMatched> OnOrderMatched;
    
    /// <summary>
    /// Event called when OrderInserted message received from WebSocket. EventId: 451
    /// </summary>
    event Action<OrderInserted> OnOrderInserted;
    
    /// <summary>
    /// Event called when OrderDeleted message received from WebSocket. EventId: 452
    /// </summary>
    event Action<OrderDeleted> OnOrderDeleted;
    
    /// <summary>
    /// Event called when UserTrade message received from WebSocket. EventId: 423
    /// </summary>
    event Action<UserTrade> OnUserTrade;
    
    /// <summary>
    /// Starts the WebSocket client.
    /// </summary>
    /// <param name="options">BtcTurk WebSocket options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task StartSocketClientAsync(BtcTurkWebSocketOptions options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Stops the WebSocket client.
    /// </summary>
    Task StopSocketClientAsync();

    /// <summary>
    /// Restarts the WebSocket client.
    /// </summary>
    Task RestartSocketClientAsync();
}