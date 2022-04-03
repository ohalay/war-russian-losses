using Cronos;
using HtmlAgilityPack;
using System.Text.Json;

namespace War.RussianLosses.Api
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
            // every day at 10am utc
            await WaitForNextScheduleAsync("0 10 * * *");
            _logger.LogInformation("Start load last losses.");

            var entities = _lossesDataLoader.LastLosses(new HtmlWeb().Load("https://index.minfin.com.ua/ua/russian-invading/casualties/"));

            _logger.LogInformation($"Last losses: {JsonSerializer.Serialize(entities.Select(s => $"{s.Date} {s.LossTypeId} {s.Count}"))}");

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WarContext>();

            await context.AddRangeAsync(entities);
            await context.SaveChangesAsync();

            _logger.LogInformation("Saved last losses.");
        }

        private async Task WaitForNextScheduleAsync(string cronExpression)
        {
            var parsedExp = CronExpression.Parse(cronExpression);
            var currentUtcTime = DateTimeOffset.UtcNow.UtcDateTime;
            var occurenceTime = parsedExp.GetNextOccurrence(currentUtcTime);

            var delay = occurenceTime.GetValueOrDefault() - currentUtcTime;

            await Task.Delay(delay);
        }
    }
}
