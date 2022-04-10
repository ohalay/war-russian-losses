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
            // every day at 7:00am utc
            var scedule = CronExpression.Parse("0 7 * * *");
            var offset = 30;
            var repitedCount = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                await WaitForNextScheduleAsync(scedule, repitedCount * offset, stoppingToken);
                try
                {
                    repitedCount = await RunAsync(stoppingToken)
                        ? 0
                        : ++repitedCount;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        private async Task<bool> RunAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Start load last losses.");

            var entities = _lossesDataLoader.LastLosses(new HtmlWeb().Load("https://index.minfin.com.ua/ua/russian-invading/casualties/"));

            _logger.LogInformation($"Last losses: {JsonSerializer.Serialize(entities.Select(s => $"{s.Date} {s.LossTypeId} {s.Count}"))}");

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WarContext>();

            await context.AddRangeAsync(entities, stoppingToken);
            await context.SaveChangesAsync(stoppingToken);

            _logger.LogInformation("Saved last losses.");

            return entities.Any();
        }

        private async Task WaitForNextScheduleAsync(CronExpression scedule, double extraMinutes, CancellationToken stoppingToken)
        {
            var currentUtcTime = DateTimeOffset.UtcNow.UtcDateTime;
            var occurenceTime = scedule.GetNextOccurrence(currentUtcTime);

            var delay = occurenceTime.GetValueOrDefault().AddMinutes(extraMinutes) - currentUtcTime;

            await Task.Delay(delay, stoppingToken);
        }
    }
}
