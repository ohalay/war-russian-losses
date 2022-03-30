using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Path = System.IO.Path;

namespace War.RussianLosses.Api
{
    public class ImageBuilder
    {
        private static readonly string _assetsPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");
        
        private readonly WarContext _warContext;

        public ImageBuilder(WarContext warContext)
        {
            _warContext = warContext;
        }

        public async Task<Stream> BuildImgStreamAsync(DateOnly? from, DateOnly? to)
        {
            
            using var originalImg = await Image.LoadAsync(Path.Combine(_assetsPath, "placeholder.jpg"));

            originalImg.Mutate(ctx => ctx.DrawText(
                "Losses",
                GetFont(30),
                Color.White,
                new Point (50,50)
            ));
            
            var memoryStream = new MemoryStream();
            await originalImg.SaveAsJpegAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }

        private static Font GetFont(float size)
        {
            FontCollection collection = new();
            var family = collection.Add(Path.Combine(_assetsPath, "OpenSans.ttf"));

            return family.CreateFont(size, FontStyle.Regular);
        }
    }
}
