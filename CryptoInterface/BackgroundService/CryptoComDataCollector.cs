using System.Diagnostics;
using CryptoInterface.CryptoCom;
using CryptoInterface.CryptoCom.Deciders;
using CryptoInterface.CryptoCom.ResponseHandlers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;

namespace CryptoInterface.BackgroundService;

public class CryptoComDataCollector : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly CryptoComMarketClient _marketClient;
    private readonly IServiceProvider _serviceProvider;

    public CryptoComDataCollector(CryptoComMarketClient marketClient, IServiceProvider serviceProvider)
    {
        _marketClient = marketClient;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _marketClient.Connect(stoppingToken);
        Console.WriteLine("Connected");
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
            using (var scope = _serviceProvider.CreateScope())
            {
                // Heartbeat check
                var hHandler = scope.ServiceProvider.GetRequiredService<HeartbeatHandler>();
                if(hHandler.CanExecute(jobj))
                {
                    await hHandler.Execute(jobj, _marketClient, stoppingToken);
                    //Console.WriteLine("Heart beaten");
                    continue;
                }

                var decider = scope.ServiceProvider.GetRequiredService<CryptoComMarketDtoDecider>();
                var val = await decider.Execute(jobj, _marketClient, stoppingToken);
                    // .ContinueWith(t => Trace.WriteLine(t.Exception), TaskContinuationOptions.NotOnRanToCompletion);
                //Console.WriteLine("{0} handlers have been used", val);
            }
            //Console.WriteLine(dto);
        }
    }

    public async Task Send(string dto, CancellationToken stoppingToken)
    {
        await _marketClient.Send(dto, stoppingToken);
    }
}