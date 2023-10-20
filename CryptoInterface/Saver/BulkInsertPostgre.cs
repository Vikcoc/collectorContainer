using System.Data;
using System.Dynamic;
using Dapper;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;

namespace CryptoInterface.Saver;

public class BulkInsertPostgre
{
    private readonly IDbConnection _connection;
    private const string InsertQuery = "INSERT INTO NewMarketSnaps (\"High\", \"Low\", \"Actual\", \"Instrument\", \"Volume\", \"UsdVolume\", \"OpenInterest\", \"Change\", \"BestBid\", \"BestBidSize\", \"BestAsk\", \"BestAskSize\", \"TradeTimestamp\", \"Timestamp\") VALUES ";

    public BulkInsertPostgre(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task Execute(IEnumerable<(CryptoComTickerData, DateTime)> data){
        var paramIndex = 1;
        var theData = data.Select(item => {
                var res = new Dictionary<string, object>
                {
                    {"@param" + (paramIndex + 0), item.Item1.High},
                    {"@param" + (paramIndex + 1), item.Item1.Low},
                    {"@param" + (paramIndex + 2), item.Item1.Actual},
                    {"@param" + (paramIndex + 3), item.Item1.Instrument},
                    {"@param" + (paramIndex + 4), item.Item1.Volume},
                    {"@param" + (paramIndex + 5), item.Item1.UsdVolume},
                    {"@param" + (paramIndex + 6), item.Item1.OpenInterest},
                    {"@param" + (paramIndex + 7), item.Item1.Change},
                    {"@param" + (paramIndex + 8), item.Item1.BestBid},
                    {"@param" + (paramIndex + 9), item.Item1.BestBidSize},
                    {"@param" + (paramIndex + 10), item.Item1.BestAsk},
                    {"@param" + (paramIndex + 11), item.Item1.BestAskSize},
                    {"@param" + (paramIndex + 12), item.Item1.TradeTimestamp},
                    {"@param" + (paramIndex + 13), item.Item2},
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