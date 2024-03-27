using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace BtcTurkApiCore.Handlers;

public class AuthenticatedHttpClientHandler :  DelegatingHandler
{
    private readonly string _publicKey;
    private readonly string _privateKey;

    public AuthenticatedHttpClientHandler(string publicKey, string privateKey)
    {
        _publicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
        _privateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        long nonce = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        string message = _publicKey + nonce;
        using (HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(_privateKey)))
        {
            byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            string signature = Convert.ToBase64String(signatureBytes);

            request.Headers.Add("X-PCK", _publicKey);
            request.Headers.Add("X-Signature", signature);
            request.Headers.Add("X-Stamp", nonce.ToString());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await base.SendAsync(request, cancellationToken);
        }
    }
}