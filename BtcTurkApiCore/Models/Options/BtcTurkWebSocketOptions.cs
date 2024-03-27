namespace BtcTurkApiCore.Models.Options;

public class BtcTurkWebSocketOptions
{
    /// <summary>
    /// BtcTurk WebSocket Public Key
    /// </summary>
    public string PublicKey { get; set; }

    /// <summary>
    /// BtcTurk WebSocket Private Key
    /// </summary>
    public string PrivateKey { get; set; }

    /// <summary>
    /// If true, the WebSocket client will automatically reconnect when the connection is closed. Default is true.
    /// </summary>
    public bool AutoReconnect { get; set; } = true;

    /// <summary>
    /// Websocket message buffer size for receive messages. I recommend buffer to be higher than 100 kb. Default is 102400 bytes (100 kb).
    /// </summary>
    public long ReceiveMessageBuffer { get; set; } = 1024 * 100;
}