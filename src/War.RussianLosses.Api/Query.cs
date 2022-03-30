using Microsoft.EntityFrameworkCore;

namespace War.RussianLosses.Api
{
    public class Query
    {
        [UseProjection, UseFiltering, UseSorting]
        public IQueryable<LossType> GetLossType([Service] WarContext ctx) =>
            ctx.LossTypes;

        [UseProjection, UseFiltering, UseSorting]
        public IQueryable<RussinLoss> GetRussinLosses([Service]WarContext ctx) =>
           ctx.RussinLosses.Include(s => s.LosType);
    }
}