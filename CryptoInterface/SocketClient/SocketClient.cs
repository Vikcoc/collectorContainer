using System.Net.WebSockets;
using System.Text;

namespace OWT.SocketClient;

public class SocketClient : ISocketClient
{
    private readonly ClientWebSocket _clientWebSocket;

    public SocketClient(ClientWebSocket clientWebSocket)
    {
        _clientWebSocket = clientWebSocket;
    }

    bool ISocketClient.IsConnected => _clientWebSocket.State == WebSocketState.Open;

    async Task ISocketClient.Connect(string path, CancellationToken token)
    {
        if (((ISocketClient)this).IsConnected)
            throw new NotImplementedException();
        await _clientWebSocket.ConnectAsync(new Uri(path), token);
    }

    async Task<string> ISocketClient.Receive(CancellationToken token)
    {
        var buffer = new ArraySegment<byte>(new byte[2048]);

        await using var objectStream = new MemoryStream();
        {
            var streamReader = new StreamReader(objectStream);


            var receiveResult = await _clientWebSocket.ReceiveAsync(buffer, token);
            while (!receiveResult.EndOfMessage && !token.IsCancellationRequested)
            {
                await objectStream.WriteAsync(buffer, token);
                receiveResult = await _clientWebSocket.ReceiveAsync(buffer, token);
            }

            await objectStream.WriteAsync(buffer.Array!, 0, receiveResult.Count, token);

            objectStream.Seek(0, SeekOrigin.Begin);
            return await streamReader.ReadToEndAsync(token);
        }
    }

    async Task ISocketClient.Send(string message, CancellationToken token)
    {
        await _clientWebSocket.SendAsync(Encoding.ASCII.GetBytes(message), WebSocketMessageType.Text, true, token);
    }
}