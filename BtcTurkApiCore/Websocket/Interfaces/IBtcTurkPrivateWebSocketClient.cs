using BtcTurkApiCore.Models.Options;
using BtcTurkApiCore.Models.WebSocket;
using BtcTurkApiCore.Models.WebSocket.Responses;

namespace BtcTurkApiCore.Websocket.Interfaces;

public interface IBtcTurkPrivateWebSocketClient
{
    event Action<OrderMatched> OnOrderMatched;
    
    event Action<OrderInserted> OnOrderInserted;
    
    event Action<OrderDeleted> OnOrderDeleted;
    
    event Action<OrderUpdated> OnOrderUpdated;
    
    event Action<UserTrade> OnUserTrade;
    
    Task StartSocketClientAsync(BtcTurkApiOptions options, CancellationToken cancellationToken = default);
    
    Task StopSocketClientAsync();

    Task RestartSocketClientAsync();
}