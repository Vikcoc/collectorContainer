using Newtonsoft.Json.Linq;

namespace OWT.CryptoCom;

public interface ICryptoComDtoExecutor
{
    bool CanExecute(JObject dto);
    Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token);
}