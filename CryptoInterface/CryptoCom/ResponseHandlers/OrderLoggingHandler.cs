using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OWT.CryptoCom.ResponseHandlers {
    public class OrderLoggingHandler : ICryptoComDtoExecutor
    {
        private readonly ILogger<OrderLoggingHandler> _logger;

        public OrderLoggingHandler(ILogger<OrderLoggingHandler> logger)
        {
            _logger = logger;
        }

        public bool CanExecute(JObject dto) => dto["method"]?.ToString() == "private/create-order";

        public Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
        {
            _logger.LogWarning(JsonConvert.SerializeObject(dto));
            return Task.CompletedTask;
        }
    }
}
