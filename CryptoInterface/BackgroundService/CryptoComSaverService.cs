using System.Collections;
using System.Diagnostics;
using CryptoInterface.CryptoCom;
using CryptoInterface.CryptoCom.ResponseHandlers;
using CryptoInterface.Saver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;

namespace CryptoInterface.BackgroundService;

public class CryptoComSaverService : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly Queue<CryptoComTickerData> _messageQueue;
    private readonly BulkInsertPostgreQuick _savePostgre;

    public CryptoComSaverService(Queue<CryptoComTickerData> messageQueue, BulkInsertPostgreQuick savePostgre)
    {
        _messageQueue = messageQueue;
        _savePostgre = savePostgre;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            var received = _messageQueue.Count;
            var obj = _messageQueue.DequeueChunk(received);
            await _savePostgre.Execute(obj);
        }
    }
}