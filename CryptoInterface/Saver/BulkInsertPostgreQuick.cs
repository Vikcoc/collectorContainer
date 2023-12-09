using System.Data;
using Dapper;
using OWT.CryptoCom.Dto;

namespace CryptoInterface.Saver;

public class BulkInsertPostgreQuick
{
    private readonly IDbConnection _connection;
    private const string InsertQuery = "INSERT INTO new_market_snaps (high, low, actual, instrument, volume, usd_volume, change, best_bid, best_bid_size, best_ask, best_ask_size, trade_timestamp) VALUES ";

    public BulkInsertPostgreQuick(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task Execute(IEnumerable<CryptoComTickerData> data){
        var paramIndex = 1;
        var theData = data.Select(item => {
                var res = new Dictionary<string, object>
                {
                    {"@param" + (paramIndex + 0), item.High},
                    {"@param" + (paramIndex + 1), item.Low},
                    {"@param" + (paramIndex + 2), item.Actual},
                    {"@param" + (paramIndex + 3), item.Instrument},
                    {"@param" + (paramIndex + 4), item.Volume},
                    {"@param" + (paramIndex + 5), item.UsdVolume},
                    {"@param" + (paramIndex + 7), item.Change},
                    {"@param" + (paramIndex + 8), item.BestBid},
                    {"@param" + (paramIndex + 9), item.BestBidSize},
                    {"@param" + (paramIndex + 10), item.BestAsk},
                    {"@param" + (paramIndex + 11), item.BestAskSize},
                    {"@param" + (paramIndex + 12), item.TradeTimestamp}
                };
                paramIndex += 14;
                return res;
            }).ToList();
        if(theData.Count < 1)
            return;
        var bulkEnding = theData.Select(x => "(" + x.Select(y => y.Key).Aggregate((a, b) => a + "," + b) + ")").ToList();
        var dbArgs = new DynamicParameters();
        foreach(var pair in theData.SelectMany(x => (IEnumerable<KeyValuePair<string, object>>) x))
            dbArgs.Add(pair.Key, pair.Value);
        var stuffToInsert = bulkEnding.Aggregate((a, b) => a + ",\n" + b);
        // Console.WriteLine(stuffToInsert);
        await _connection.ExecuteAsync(InsertQuery + stuffToInsert, dbArgs);
        // Console.WriteLine(theData.Count);
    }
}