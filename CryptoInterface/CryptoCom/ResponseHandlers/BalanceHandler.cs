using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;

namespace OWT.CryptoCom.ResponseHandlers;

public class BalanceHandler : ICryptoComDtoExecutor
{
    private readonly CryptoComBalanceDto _balanceDto;

    public BalanceHandler(CryptoComBalanceDto balanceDto)
    {
        _balanceDto = balanceDto;
    }

    public bool CanExecute(JObject dto)
    {
        return dto["method"]?.ToString() == "subscribe" && dto["result"]?["channel"]?.ToString() == "user.balance";
    }

    public Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
    {
        foreach (var value in dto["result"]?["data"]?.Values<JObject>() ?? Array.Empty<JObject>())
        {
            var prop = typeof(CryptoComBalanceDto).GetProperties().FirstOrDefault(x =>
                (x.GetCustomAttribute(typeof(JsonPropertyAttribute)) as JsonPropertyAttribute)?.PropertyName ==
                value?["currency"]?.ToObject<string>());
            if (prop == null)
                continue;
            prop.SetValue(_balanceDto, value?["available"]?.ToObject<double>() ?? 0);
        }

        return Task.CompletedTask;
    }
}