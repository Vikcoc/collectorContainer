using Newtonsoft.Json.Linq;

namespace CryptoInterface.CryptoCom;

public interface ICryptoComDtoExecutor
{
    bool CanExecute(JObject dto);
    Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token);
}