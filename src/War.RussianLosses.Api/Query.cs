using Microsoft.EntityFrameworkCore;
using War.RussianLosses.Api.Services;
using static War.RussianLosses.Api.Services.LossesAmountService;

namespace War.RussianLosses.Api
{
    public class Query
    {
        [UseProjection, UseFiltering, UseSorting]
        public IQueryable<LossType> GetLossType(WarContext ctx)
            => ctx.LossTypes;

        [UseProjection, UseFiltering, UseSorting]
        public IQueryable<RussinLoss> GetRussinLosses(WarContext ctx)
            => ctx.RussinLosses.Include(s => s.LossType);

        public Task<List<RusionLossesAmount>> GetRussinLossesAmount([Service]LossesAmountService svc, DateOnly? from, DateOnly? to)
            => svc.GetLossesAmountAsync(from, to);
    }
}