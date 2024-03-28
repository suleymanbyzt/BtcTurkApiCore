namespace BtcTurkApiCore.Models.Options;

public class BtcTurkApiOptions
{
    /// <summary>
    /// BtcTurk API Public Key
    /// </summary>
    public string PublicKey { get; set; }

    /// <summary>
    /// BtcTurk API Private Key
    /// </summary>
    public string PrivateKey { get; set; }

    /// <summary>
    /// Delay tolerance for each request. Default is 15000ms (15 seconds).
    /// </summary> 
    public long Nonce { get; set; } = 15000;
}