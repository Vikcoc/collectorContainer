using System.Diagnostics;
using CryptoInterface.CryptoCom;
using CryptoInterface.CryptoCom.ResponseHandlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;

namespace CryptoInterface.BackgroundService;

public class CryptoComSimpleCollector : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly CryptoComMarketClient _marketClient;
    private readonly HeartbeatHandler _heartbeatHandler;
    private readonly TickerSaveHandlerPostgre _savePostgre;

    public CryptoComSimpleCollector(CryptoComMarketClient marketClient, HeartbeatHandler heartbeatHandler, TickerSaveHandlerPostgre savePostgre)
    {
        _marketClient = marketClient;
        _heartbeatHandler = heartbeatHandler;
        _savePostgre = savePostgre;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _marketClient.Connect(stoppingToken);
        Console.WriteLine("Connected " + DateTime.UtcNow.ToString());
        var trans = new CryptoComParamTransaction
        {
            Method = "subscribe",
            Params = new Dictionary<string, object>
            {
                { "channels", new[] { "ticker.ETH_USDT" } }
            }
        };
        await _marketClient.Send(JsonConvert.SerializeObject(trans), stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            var dto = await _marketClient.Receive(stoppingToken);
            var jobj = JsonConvert.DeserializeObject<JObject>(dto)!;

            if (_heartbeatHandler.CanExecute(jobj))
            {
                await _heartbeatHandler.Execute(jobj, _marketClient, stoppingToken);
                continue;
            }

            if (_savePostgre.CanExecute(jobj))
                _savePostgre.Execute(jobj, _marketClient, stoppingToken).ContinueWith(t => Trace.WriteLine(t.Exception),TaskContinuationOptions.NotOnRanToCompletion); 
        }
    }

    public async Task Send(string dto, CancellationToken stoppingToken)
    {
        await _marketClient.Send(dto, stoppingToken);
    }
}