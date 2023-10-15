using Newtonsoft.Json;

namespace OWT.CryptoCom.Dto
{
    public class CryptoComTickerData
    {
        [JsonProperty("h")]
        public decimal High { get; set; }

        [JsonProperty("l")]
        public decimal Low { get; set; }

        [JsonProperty("a")]
        public decimal Actual { get; set; }

        [JsonProperty("i")] public string Instrument { get; set; } = string.Empty;

        [JsonProperty("v")]
        public decimal Volume { get; set; }

        [JsonProperty("vv")]
        public decimal UsdVolume { get; set; }

        [JsonProperty("oi")]
        public decimal OpenInterest { get; set; }

        [JsonProperty("c")]
        public decimal Change { get; set; }

        [JsonProperty("b")]
        public decimal BestBid { get; set; }

        [JsonProperty("bs")]
        public decimal BestBidSize { get; set; }

        [JsonProperty("k")]
        public decimal BestAsk { get; set; }

        [JsonProperty("ks")]
        public decimal BestAskSize { get; set; }

        [JsonProperty("t")]
        public long TradeTimestamp { get; set; }
    }
}
