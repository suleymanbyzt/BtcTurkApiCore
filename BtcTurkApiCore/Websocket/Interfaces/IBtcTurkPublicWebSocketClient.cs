using BtcTurkApiCore.Constants;
using BtcTurkApiCore.Models.WebSocket;
using BtcTurkApiCore.Models.WebSocket.Responses;

namespace BtcTurkApiCore.Websocket.Interfaces;

public interface IBtcTurkPublicWebSocketClient
{
    event Action<TickerAll> OnTickerAll;
    
    event Action<TickerPair> OnTickerPair;
    
    event Action<OrderBookFull> OnOrderBook;
    
    event Action<TradeSingle> OnTradeSingle;
    
    event Action<TradingView> OnTradingView;
    
    Task StartSocketClientAsync(CancellationToken cancellationToken = default);

    Task StopSocketClientAsync();
    
    Task RestartSocketClientAsync();
    
    Task SendSubscriptionRequest(Channels channel, string @event);
}