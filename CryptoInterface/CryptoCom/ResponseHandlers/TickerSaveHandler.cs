using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace OWT.CryptoCom.ResponseHandlers;

public class TickerSaveHandler : ICryptoComDtoExecutor
{
    private const string InsertQuery =
        "INSERT INTO [MarketStateSnaps] ([DateTime], [InstrumentName], [BestBid], [BestAsk], [Actual], [Low], [High], [Volume], [Change], [Timestamp], [BigVolume], [PartChange]) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10, @param11, @param12)";

    private const string SelectQuery =
        "SELECT TOP (1) [Actual] AS Value FROM [MarketStateSnaps] ORDER BY [DateTime] DESC";

    private readonly IDbConnection _connection;
    private readonly ILogger<TickerSaveHandler> _logger;

    public TickerSaveHandler(IDbConnection connection, ILogger<TickerSaveHandler> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public bool CanExecute(JObject dto)
    {
        return dto["method"]?.ToString() == "subscribe" && dto["result"]?["channel"]?.ToString() == "ticker";
    }

    public Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
    {
        var val = dto["result"]?["data"]?.Values<JObject>().FirstOrDefault();
        if (val?["a"]?.Value<double?>() == _connection.Query<double?>(SelectQuery).FirstOrDefault())
            return Task.CompletedTask;
        var time = DateTime.UtcNow;
        _connection.Execute(InsertQuery, new
        {
            param1 = time,
            param2 = dto["result"]?["instrument_name"]?.ToString(),
            param3 = val?["b"]?.Value<double>(),
            param4 = val?["k"]?.Value<double>(),
            param5 = val?["a"]?.Value<double>(),
            param6 = val?["l"]?.Value<double>(),
            param7 = val?["h"]?.Value<double>(),
            param8 = val?["v"]?.Value<double>(),
            param9 = val?["c"]?.Value<double>(),
            param10 = val?["t"]?.Value<long>(),
            param11 = val?["vv"]?.Value<double>(),
            param12 = val?["pc"]?.Value<double>()
        });

        _logger.LogInformation("Inserted {0} at timestamp {1}", val?["a"]?.Value<double?>(), time);
        return Task.CompletedTask;
    }
}