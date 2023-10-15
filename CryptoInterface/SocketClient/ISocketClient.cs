namespace OWT.SocketClient;

public interface ISocketClient
{
    bool IsConnected { get; }
    Task Connect(string path, CancellationToken token);
    Task<string> Receive(CancellationToken token);
    Task Send(string message, CancellationToken token);
}