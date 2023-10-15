using Newtonsoft.Json.Linq;

namespace OWT.CryptoCom.Deciders;

public class CryptoComDtoDecider
{
    private readonly IList<ICryptoComDtoExecutor> _cryptoComDtoExecutors;

    public CryptoComDtoDecider()
    {
        _cryptoComDtoExecutors = new List<ICryptoComDtoExecutor>();
    }

    public void AddHandler(ICryptoComDtoExecutor executor)
    {
        _cryptoComDtoExecutors.Add(executor);
    }

    public async Task<int> Execute(JObject? dto, CryptoComMarketClient marketClient,
        CancellationToken cancellationToken)
    {
        if (dto == null)
            return 0;
        var cnt = 0;
        var index = 0;
        while (!cancellationToken.IsCancellationRequested && index < _cryptoComDtoExecutors.Count)
        {
            if (_cryptoComDtoExecutors[index].CanExecute(dto))
            {
                await _cryptoComDtoExecutors[index].Execute(dto, marketClient, cancellationToken);
                cnt++;
            }

            index++;
        }

        return cnt;
    }
}