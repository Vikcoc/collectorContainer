using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;
using System.Data;

namespace CryptoInterface.CryptoCom.ResponseHandlers;

public class TickerSaveHandlerPostgre : ICryptoComDtoExecutor
{
    private readonly ILogger<TickerSaveHandlerPostgre> _logger;

    private readonly Queue<CryptoComTickerData> _messageQueue;

    private decimal prev;

    public TickerSaveHandlerPostgre(ILogger<TickerSaveHandlerPostgre> logger, Queue<CryptoComTickerData> messageQueue)
    {
        _logger = logger;
        _messageQueue = messageQueue;
    }

    public bool CanExecute(JObject dto) => dto["method"]?.ToString() == "subscribe" && dto["result"]?["channel"]?.ToString() == "ticker";

    public Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
    {
        var data = dto?["result"]?["data"]?.ToObject<CryptoComTickerData[]>();
        if (data == null)
        {
            _logger.LogInformation("Received no data from socket");
            return Task.CompletedTask;
        }
        foreach (var item in data)
        {
            if (item.Actual == prev)
                continue;
            
            prev = item.Actual;
            _messageQueue.Enqueue(item);
        }
        return Task.CompletedTask;
    }
}
