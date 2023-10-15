using Newtonsoft.Json.Linq;

namespace OWT.CryptoCom.ResponseHandlers;

public class TickerHandler : ICryptoComDtoExecutor
{
    public bool CanExecute(JObject dto)
    {
        return dto["method"]?.ToString() == "subscribe" && dto["result"]?["channel"]?.ToString() == "ticker";
    }


    public Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
    {
        Console.WriteLine("a");
        return Task.CompletedTask;
    }
}