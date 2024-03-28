using System.Net.WebSockets;
using System.Text;
using BtcTurkApiCore.Constants;
using BtcTurkApiCore.Extensions;
using BtcTurkApiCore.Models.WebSocket.Responses;
using BtcTurkApiCore.Websocket.Interfaces;

namespace BtcTurkApiCore.Websocket;

public class BtcTurkPublicWebsocketClient : IBtcTurkPublicWebSocketClient
{
    private ClientWebSocket _clientWebSocket;
    private Uri _uri = new Uri("wss://ws-feed-pro.btcturk.com");
    private readonly TaskCompletionSource _socketTaskCompletionSource = new TaskCompletionSource();
    private bool _isSocketStopped = false;

    public event Action<TickerAll>? OnTickerAll;
    public event Action<TickerPair>? OnTickerPair;
    public event Action<OrderBookFull>? OnOrderBook;
    public event Action<TradeSingle>? OnTradeSingle;
    public event Action<TradingView>? OnTradingView;
    public event Action<Exception>? OnError; 

    public async Task StartSocketClientAsync(CancellationToken cancellationToken = default)
    {
        _isSocketStopped = false;
        
        _clientWebSocket = new ClientWebSocket();
        await _clientWebSocket.ConnectAsync(_uri, cancellationToken);

        _ = Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested && !_isSocketStopped)
            {
                try
                {
                    if (_clientWebSocket.State != WebSocketState.Open)
                    {
                        _clientWebSocket = new ClientWebSocket();
                        await _clientWebSocket.ConnectAsync(_uri, cancellationToken);
                    }
                
                    await GetSocketMessages(cancellationToken);
                }
                finally
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
        }, cancellationToken);

        await _socketTaskCompletionSource.Task;
    }

    public Task StopSocketClientAsync()
    {
        _clientWebSocket.Dispose();
        _isSocketStopped = true;
        return Task.CompletedTask;
    }

    public async Task RestartSocketClientAsync()
    {
        await StopSocketClientAsync();
        await StartSocketClientAsync();
    }

    public async Task SendSubscriptionRequest(Channels channel, string @event)
    {
        string socketJoinRequest = SocketMessageExtension.JoinRequest(channel, @event);

        await _clientWebSocket.SendAsync(buffer: new ArraySegment<byte>(
                array: Encoding.UTF8.GetBytes(socketJoinRequest),
                offset: 0,
                count: Encoding.UTF8.GetBytes(socketJoinRequest).Length),
            messageType: WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken: CancellationToken.None);
    }

    private async Task GetSocketMessages(CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[1024 * 1024];

        while (_clientWebSocket.State == WebSocketState.Open)
        {
            try
            {
                WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                string resultMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);

                int? messageId = resultMessage.SocketMessageId();

                if (messageId == 991 && !_socketTaskCompletionSource.Task.IsCompleted)
                {
                    _socketTaskCompletionSource.SetResult();
                }

                switch (messageId)
                {
                    case 401:
                        TickerAll? tickerAll = await resultMessage.GetResponse<TickerAll>();
                        if (tickerAll != null) OnTickerAll?.Invoke(tickerAll);
                        break;

                    case 402:
                        TickerPair? tickerPair = await resultMessage.GetResponse<TickerPair>();
                        if (tickerPair != null) OnTickerPair?.Invoke(tickerPair);
                        break;

                    case 422:
                        TradeSingle? tradeSingle = await resultMessage.GetResponse<TradeSingle>();
                        if (tradeSingle != null) OnTradeSingle?.Invoke(tradeSingle);
                        break;

                    case 428:
                        TradingView? tradingView = await resultMessage.GetResponse<TradingView>();
                        if (tradingView != null) OnTradingView?.Invoke(tradingView);
                        break;

                    case 431:
                        OrderBookFull? orderBook = await resultMessage.GetResponse<OrderBookFull>();
                        if (orderBook != null) OnOrderBook?.Invoke(orderBook);
                        break;
                }
            }
            catch (Exception e)
            {
                _clientWebSocket.Dispose();
                OnError?.Invoke(e);
            }
        }
    }
}