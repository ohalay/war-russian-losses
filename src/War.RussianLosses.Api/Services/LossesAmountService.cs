using Microsoft.EntityFrameworkCore;

namespace War.RussianLosses.Api.Services
{
    public class LossesAmountService : IAsyncDisposable
    {
        private readonly WarContext _warContext;

        public LossesAmountService(WarContext warContext)
        {
            _warContext = warContext;
        }

        public Task<List<RusionLossesAmount>> GetLossesAmountAsync(DateOnly? startDate, DateOnly? endDate)
        {
            return _warContext.RussinLosses
                 .Include(s => s.LossType)
                 .Where(s => startDate == null || s.Date >= startDate)
                 .Where(s => endDate == null || s.Date <= endDate)
                 .GroupBy(s => new { s.LossType.Name, s.LossType.NameEnglish })
                 .Select(g => new RusionLossesAmount(g.Key.Name, g.Key.NameEnglish, g.Sum(s => s.Count)))
                 .ToListAsync();
        }

        public ValueTask DisposeAsync()
            => _warContext.DisposeAsync();

        public record RusionLossesAmount(string name, string nameEnglish, int count);
    }
}
