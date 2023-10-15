using Newtonsoft.Json;

namespace OWT.CryptoCom.Dto;

public class CryptoComBalanceDto
{
    [JsonProperty("USDT")] public double Usd { get; set; }

    [JsonProperty("ETH")] public double Eth { get; set; }

    [JsonProperty("BTC")] public double Btc { get; set; }
}