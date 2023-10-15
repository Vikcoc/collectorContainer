using Newtonsoft.Json;
using OWT.CryptoCom.Dto;

namespace OWT.CryptoCom.Deciders
{
    public class CryptoComPurchaseDecider
    {
        protected async Task Trade(double amount, string buySell, CryptoComMarketClient marketClient, CancellationToken token)
        {
            var trans = new CryptoComParamTransaction
            {
                Method = "private/create-order",
                Params = new Dictionary<string, object>
                {
                    { "instrument_name", "ETH_USDT" },
                    { "side", buySell },
                    { "type", "MARKET" },
                    { "quantity", amount },
                }
            };
            await marketClient.Send(JsonConvert.SerializeObject(trans), token);
        }

        public async Task Buy(CryptoComMarketClient marketClient, CancellationToken token, double amount = 0.001) =>
            await Trade(amount, "BUY", marketClient, token);
        public async Task Sell(CryptoComMarketClient marketClient, CancellationToken token, double amount = 0.001) =>
            await Trade(amount, "SELL", marketClient, token);
    }
}
