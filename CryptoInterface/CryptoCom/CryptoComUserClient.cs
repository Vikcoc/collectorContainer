using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using OWT.CryptoCom.Dto;
using OWT.SocketClient;

namespace OWT.CryptoCom;

public class CryptoComUserClient : CryptoComMarketClient
{
    private readonly CryptoComCredentials _credentials;


    public CryptoComUserClient(ISocketClient socketClient, CryptoComCredentials credentials) : base(socketClient)
    {
        _credentials = credentials;
    }

    protected override string SocketEndpoint => "wss://stream.crypto.com/v2/user";

    public async Task Authenticate(CancellationToken token)
    {
        var trans = new CryptoComSignedTransaction
        {
            Method = "public/auth",
            ApiKey = _credentials.ApiKey,
            Nonce = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(20)).ToUnixTimeMilliseconds()
        };

        var sigPayload = Encoding.ASCII.GetBytes(trans.Method + trans.Id + trans.ApiKey + trans.Nonce);

        var hash = new HMACSHA256(Encoding.ASCII.GetBytes(_credentials.SecretKey));
        var computedHash = hash.ComputeHash(sigPayload);
        trans.Signature = BitConverter.ToString(computedHash).Replace("-", string.Empty);

        await Send(JsonConvert.SerializeObject(trans), token);
    }
}