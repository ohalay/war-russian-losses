using Microsoft.EntityFrameworkCore;

namespace War.RussianLosses.Api.Services
{
    public class LossesAmountService
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
                 .GroupBy(s => s.LossType.Name)
                 .Select(g => new RusionLossesAmount(g.Key, g.Sum(s => s.Count)))
                 .ToListAsync();
        }

        public record RusionLossesAmount(string name, int count);
    }
}
