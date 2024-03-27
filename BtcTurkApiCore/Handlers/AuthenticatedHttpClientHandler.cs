using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using BtcTurkApiCore.Models.Options;

namespace BtcTurkApiCore.Handlers;

public class AuthenticatedHttpClientHandler :  DelegatingHandler
{
    private readonly BtcTurkApiOptions _btcTurkApiOptions;

    public AuthenticatedHttpClientHandler(string publicKey, string privateKey, long nonce)
    {
        _btcTurkApiOptions = new BtcTurkApiOptions
        {
            PublicKey = publicKey,
            PrivateKey = privateKey,
            Nonce = nonce
        };
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        long nonce = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        string message = _btcTurkApiOptions.PublicKey + _btcTurkApiOptions.Nonce;
        using (HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(_btcTurkApiOptions.PrivateKey)))
        {
            byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            string signature = Convert.ToBase64String(signatureBytes);

            request.Headers.Add("X-PCK", _btcTurkApiOptions.PrivateKey);
            request.Headers.Add("X-Signature", signature);
            request.Headers.Add("X-Stamp", nonce.ToString());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await base.SendAsync(request, cancellationToken);
        }
    }
}