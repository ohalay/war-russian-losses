namespace War.RussianLosses.Api.HosedServices
{
    /// <summary>
    /// We host solution to Heroku cloud and it have idle timeout - 30m.
    /// To fix it we create wormer service that ping himself every 25m.
    /// </summary>
    public class WarmerHostedService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WarmerHostedService> _logger;

        public WarmerHostedService(
            IHttpClientFactory httpClientFactory,
            ILogger<WarmerHostedService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(25), stoppingToken);
                try
                {
                    await PingAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        private async Task PingAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Start service warmer.");

            using var client = _httpClientFactory.CreateClient(Const.SelfHttpClientName);
            await client.GetAsync("hc", stoppingToken);
        }
    }
}
