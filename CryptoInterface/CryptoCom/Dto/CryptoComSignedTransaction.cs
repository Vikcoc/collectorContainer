using Newtonsoft.Json;

namespace OWT.CryptoCom.Dto;

public class CryptoComSignedTransaction : CryptoComBaseTransaction
{
    [JsonProperty("api_key")] public string ApiKey { get; set; } = string.Empty;

    [JsonProperty("sig")] public string Signature { get; set; } = string.Empty;

    [JsonProperty("nonce")] public long Nonce { get; set; }
}