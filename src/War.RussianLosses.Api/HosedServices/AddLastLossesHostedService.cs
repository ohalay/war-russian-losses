using Cronos;
using HtmlAgilityPack;
using System.Text.Json;
using War.RussianLosses.Api.Services;

namespace War.RussianLosses.Api.HosedServices
{
    public class AddLastLossesHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly LossesDataLoader _lossesDataLoader;
        private readonly ILogger<AddLastLossesHostedService> _logger;

        public AddLastLossesHostedService(
            IServiceProvider serviceProvider,
            LossesDataLoader lossesDataLoader,
            ILogger<AddLastLossesHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _lossesDataLoader = lossesDataLoader;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // every day at 7:30am utc
                await WaitForNextScheduleAsync("30 7 * * *", stoppingToken);
                try
                {
                    await RunAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        private async Task RunAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Start load last losses.");

            var entities = _lossesDataLoader.LastLosses(new HtmlWeb().Load("https://index.minfin.com.ua/ua/russian-invading/casualties/"));

            _logger.LogInformation($"Last losses: {JsonSerializer.Serialize(entities.Select(s => $"{s.Date} {s.LossTypeId} {s.Count}"))}");

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WarContext>();

            await context.AddRangeAsync(entities);
            await context.SaveChangesAsync(stoppingToken);

            _logger.LogInformation("Saved last losses.");
        }

        private async Task WaitForNextScheduleAsync(string cronExpression, CancellationToken stoppingToken)
        {
            var parsedExp = CronExpression.Parse(cronExpression);
            var currentUtcTime = DateTimeOffset.UtcNow.UtcDateTime;
            var occurenceTime = parsedExp.GetNextOccurrence(currentUtcTime);

            var delay = occurenceTime.GetValueOrDefault() - currentUtcTime;

            await Task.Delay(delay, stoppingToken);
        }
    }
}
