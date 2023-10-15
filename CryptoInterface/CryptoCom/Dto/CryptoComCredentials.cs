namespace OWT.CryptoCom.Dto;

public class CryptoComCredentials
{
    public CryptoComCredentials(string apiKey, string secretKey)
    {
        ApiKey = apiKey;
        SecretKey = secretKey;
    }

    public string ApiKey { get; }
    public string SecretKey { get; }
}