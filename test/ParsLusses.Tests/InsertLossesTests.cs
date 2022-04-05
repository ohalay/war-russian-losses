using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using War.RussianLosses.Api.Services;
using Xunit;

namespace ParsLusses.Tests
{
    public class InsertLossesTests : IDisposable
    {
        private readonly WarContext _context;
        private readonly LossesDataLoader _loader;

        public InsertLossesTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(GetType().Assembly)
                .Build();

            var contextBuilder = new DbContextOptionsBuilder<WarContext>()
                .UseNpgsql(configuration.GetConnectionString("Postgre"));

            _context = new WarContext(contextBuilder.Options);
            _loader = new LossesDataLoader();
        }


        [Fact(Skip = "Insert initial data")]
        public async Task InserInitialData()
        {
            var path = Path.Combine("assets", "losses-initial.txt");
            var losses = await _loader.ParsFromFileAsync(path);

            await _context.AddRangeAsync(losses);
            await _context.SaveChangesAsync();
        }

        [Fact(Skip = "Insert delta")]
        public async Task InserDelta()
        {
            DateOnly skipData = DateOnly.ParseExact("02/04/2022", "dd/MM/yyyy");
            var path = Path.Combine("assets", "losses-delta.txt");
            var losses = await _loader.ParsFromFileAsync(path);

            await _context.AddRangeAsync(losses.SkipWhile(s => s.Date == skipData));
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}