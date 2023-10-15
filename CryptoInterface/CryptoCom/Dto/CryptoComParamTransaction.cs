using Newtonsoft.Json;

namespace OWT.CryptoCom.Dto;

public class CryptoComParamTransaction : CryptoComBaseTransaction
{
    [JsonProperty("params")] public Dictionary<string, object>? Params { get; set; }
}