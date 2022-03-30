using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Path = System.IO.Path;

namespace War.RussianLosses.Api
{
    public class ImageBuilder
    {
        private readonly WarContext _warContext;

        public ImageBuilder(WarContext warContext)
        {
            _warContext = warContext;
        }


        public async Task<Stream> BuildImgStreamAsync(DateOnly? from, DateOnly? to)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "assets", "placeholder.jpg");
            var font = SystemFonts.CreateFont("Arial", 50);
            using var originalImg = await Image.LoadAsync(path);

            originalImg.Mutate(ctx => ctx.DrawText("Losses", font, Color.White, new Point (50,50)));
            
            var memoryStream = new MemoryStream();
            await originalImg.SaveAsJpegAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;

        }
    }
}
