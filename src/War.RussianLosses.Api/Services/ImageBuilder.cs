using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Path = System.IO.Path;

namespace War.RussianLosses.Api.Services
{
    public class ImageBuilder
    {
        private static readonly string _assetsPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");

        private readonly LossesAmountService _lossesAmountService;

        public ImageBuilder(LossesAmountService lossesAmountService)
        {
            _lossesAmountService = lossesAmountService;
        }

        public async Task<Stream> BuildImgStreamAsync(DateOnly? from, DateOnly? to)
        {
            var font = GetFont(40);
            var pen = new Pen(Color.White, 2);
            var point = new Point(200, 150);
            var offset = 60;

            using var originalImg = await Image.LoadAsync(Path.Combine(_assetsPath, "placeholder.jpg"));
            var data = await _lossesAmountService.GetLossesAmountAsync(from, to);

            originalImg.Mutate(ctx =>
            {
                ctx.DrawText(
                    new TextOptions(font) { HorizontalAlignment = HorizontalAlignment.Center, Origin = new Point(originalImg.Width / 2, 30) },
                    GetTitle(from, to),
                    pen);

                foreach (var item in data)
                {
                    ctx.DrawText(new TextOptions(font) { HorizontalAlignment = HorizontalAlignment.Right, Origin = point }, item.count.ToString(), pen);
                    ctx.DrawText(new TextOptions(font) { Origin = point }, $"   {item.name}", pen);

                    point = new Point(point.X, point.Y + offset);
                }
            });

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

        private static string GetTitle(DateOnly? from, DateOnly? to)
        {
            var text = "Орієнтовні втрати русні";
            var startDate = from ?? new DateOnly(2022, 2, 24);
            var endDate = to ?? DateOnly.FromDateTime(DateTime.Now);
            var dateFormat = "dd.MM.yy";

            return $"{text} {startDate.ToString(dateFormat)} - {endDate.ToString(dateFormat)}";
        }
    }
}
