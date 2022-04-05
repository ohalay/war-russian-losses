using HtmlAgilityPack;
using System.IO;
using War.RussianLosses.Api.Services;
using Xunit;

namespace ParsLusses.Tests
{
    public class LossesDataLoaderTests
    {
        private readonly LossesDataLoader _loader;

        public LossesDataLoaderTests()
        {
            _loader = new LossesDataLoader();
        }

        [Fact]
        public void LastLosses_FromFile_LossessLoaded()
        {
            var path = Path.Combine("assets", "losses-delta.txt");
            var doc = new HtmlDocument();
            doc.Load(path);

            var losses = _loader.LastLosses(doc);

            Assert.NotNull(losses);
        }
    }
}