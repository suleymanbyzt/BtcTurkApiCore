using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BtcTurkApiCore.Extensions;
using BtcTurkApiCore.Models.Options;
using BtcTurkApiCore.Models.WebSocket.Responses;
using BtcTurkApiCore.Websocket.Interfaces;

namespace BtcTurkApiCore.Websocket;

public class BtcTurkPrivateWebsocketClient : IBtcTurkPrivateWebSocketClient
{
    private ClientWebSocket _clientWebSocket;
    private Uri _uri = new Uri("wss://ws-feed-pro.btcturk.com");
    private BtcTurkWebSocketOptions _options = new BtcTurkWebSocketOptions();
    
    public event Action<OrderMatched>? OnOrderMatched;
    public event Action<OrderInserted>? OnOrderInserted;
    public event Action<OrderDeleted>? OnOrderDeleted;
    public event Action<UserTrade>? OnUserTrade;
    
    public async Task StartSocketClientAsync(BtcTurkWebSocketOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null || string.IsNullOrEmpty(options.PublicKey) || string.IsNullOrEmpty(options.PrivateKey))
        {
            throw new ArgumentNullException(nameof(options), "BtcTurkApiOptions is required. Please provide PublicKey and PrivateKey.");
        }

        _options = options;
        _clientWebSocket = new ClientWebSocket();
        await _clientWebSocket.ConnectAsync(_uri, cancellationToken);
        
        _ = Task.Run(async () =>
        {
            if (_options.AutoReconnect)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        _clientWebSocket = new ClientWebSocket();
                        await GetPrivateSocketMessages(cancellationToken);
                    }
                    finally
                    {
                        await Task.Delay(1000, cancellationToken);
                    }
                }
            }
            else
            {
                await GetPrivateSocketMessages(cancellationToken);
            }
        }, cancellationToken);
    }

    public Task StopSocketClientAsync()
    {
        _clientWebSocket.Dispose();
        return Task.CompletedTask;
    }

    public async Task RestartSocketClientAsync()
    {
        await StopSocketClientAsync();
        await StartSocketClientAsync(_options);
    }
    
    private async Task GetPrivateSocketMessages(CancellationToken cancellationToken)
    {
        string publicKey = _options.PublicKey;
        string privateKey = _options.PrivateKey;
        long nonce = 15000;
        string baseString = $"{publicKey}{nonce}";
        string signature = ComputeHash(privateKey, baseString);
        long timestamp = DateTime.UtcNow.ToUnixTime();
        
        object[] hmacMessageObject = { 114, new { type = 114, publicKey = publicKey, timestamp = timestamp, nonce = nonce, signature = signature } };
        
        string message = JsonSerializer.Serialize(hmacMessageObject);

        await _clientWebSocket.SendAsync(new ArraySegment<byte>(array: Encoding.UTF8.GetBytes(message),
                0,
                message.Length),
            WebSocketMessageType.Text,
            true,
            cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                string resultMessage = await ReceiveMessageAsync(_clientWebSocket, cancellationToken);

                int? messageId = resultMessage.SocketMessageId();
                if (messageId == null || messageId == 991)
                {
                    continue;
                }

                if (messageId == 114)
                {
                    UserLogin? userLoginResult = await resultMessage.GetResponse<UserLogin>();
                    if (userLoginResult != null && !userLoginResult.Ok)
                    {
                        throw new WebSocketException("Problem occurred during authentication.");
                    }
                }
                
                switch (messageId)
                {
                    case 423:
                        UserTrade? userTrade = await resultMessage.GetResponse<UserTrade>();
                        if (userTrade != null) OnUserTrade?.Invoke(userTrade);
                        break;

                    case 441:
                        OrderMatched? orderMatched = await resultMessage.GetResponse<OrderMatched>();
                        if (orderMatched != null) OnOrderMatched?.Invoke(orderMatched);
                        break;

                    case 451:
                        OrderInserted? orderInserted = await resultMessage.GetResponse<OrderInserted>();
                        if (orderInserted != null) OnOrderInserted?.Invoke(orderInserted);
                        break;

                    case 452:
                        OrderDeleted? orderDeleted = await resultMessage.GetResponse<OrderDeleted>();
                        if (orderDeleted != null) OnOrderDeleted?.Invoke(orderDeleted);
                        break;
                }
            }
            catch (Exception e)
            {
                _clientWebSocket.Dispose();
                throw new Exception("WebSocket connection closed.", e);
            }
        }
    }

    private static string ComputeHash(string privateKey, string baseString)
    {
        byte[] key = Convert.FromBase64String(privateKey);
        string hashString;

        using (HMACSHA256 hmac = new HMACSHA256(key))
        {
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(baseString));
            hashString = Convert.ToBase64String(hash);
        }

        return hashString;
    }

    private async Task<string> ReceiveMessageAsync(ClientWebSocket client, CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[_options.ReceiveMessageBuffer];

        WebSocketReceiveResult result;
        StringBuilder resultMessageBuilder = new StringBuilder();

        do
        {
            result = await client.ReceiveAsync(buffer, cancellationToken);
            string bufferMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);

            resultMessageBuilder.Append(bufferMessage);
        } while (!result.EndOfMessage);

        return resultMessageBuilder.ToString();
    }
}