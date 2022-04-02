using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ParsLusses.Tests
{
    public class InsertLossesTests : IDisposable
    {
        private readonly WarContext _context;

        public InsertLossesTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(GetType().Assembly)
                .Build();

            var contextBuilder = new DbContextOptionsBuilder<WarContext>()
                .UseNpgsql(configuration.GetConnectionString("Postgre"));

            _context = new WarContext(contextBuilder.Options);
        }


        [Fact(Skip = "Insert initial data")]
        public async Task InserInitialData()
        {
            var lusses = await new DataParser()
                .ParsFromFileAsync("losses-initial.txt");

            await _context.AddRangeAsync(lusses);
            await _context.SaveChangesAsync();
        }

        [Fact(Skip = "Insert delta")]
        public async Task InserDelta()
        {
            DateOnly skipData = DateOnly.ParseExact("01/04/2022", "dd/MM/yyyy");

            var lusses = await new DataParser()
                .ParsFromFileAsync("losses-delta.txt");

            await _context.AddRangeAsync(lusses.SkipWhile(s => s.Date == skipData));
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}