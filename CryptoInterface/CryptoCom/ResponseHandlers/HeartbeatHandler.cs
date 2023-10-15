using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;

namespace OWT.CryptoCom.ResponseHandlers;

public class HeartbeatHandler : ICryptoComDtoExecutor
{
    public bool CanExecute(JObject dto)
    {
        return dto["method"]?.ToString() == "public/heartbeat";
    }

    public async Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
    {
        var trans = new CryptoComParamTransaction
        {
            Id = long.Parse(dto["id"]?.ToString() ?? "-1"),
            Method = "public/respond-heartbeat"
        };
        await marketClient.Send(JsonConvert.SerializeObject(trans), token);
    }
}