using OWT.SocketClient;

namespace OWT.CryptoCom;

public class CryptoComMarketClient
{
    private readonly ISocketClient _socketClient;

    public CryptoComMarketClient(ISocketClient socketClient)
    {
        _socketClient = socketClient;
    }

    protected virtual string SocketEndpoint => "wss://stream.crypto.com/v2/market";

    public bool IsConnected => _socketClient.IsConnected;

    public async Task Connect(CancellationToken token)
    {
        await _socketClient.Connect(SocketEndpoint, token);
    }

    public async Task<string> Receive(CancellationToken token)
    {
        return await _socketClient.Receive(token);
    }

    public async Task Send(string message, CancellationToken token)
    {
        await _socketClient.Send(message, token);
    }
}