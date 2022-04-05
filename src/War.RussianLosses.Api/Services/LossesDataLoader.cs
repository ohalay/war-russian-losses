using HtmlAgilityPack;

namespace War.RussianLosses.Api.Services
{
    /// <summary>
    /// Data source https://www.mil.gov.ua/
    /// </summary>
    public class LossesDataLoader
    {
        private Dictionary<string, int> _nameToTypeMapper = new Dictionary<string, int>
        {
            ["Танки"] = 2,
            ["ББМ"] = 3,
            ["Літаки"] = 7,
            ["Гелікоптери"] = 8,
            ["Гармати"] = 4,
            ["Автомобілі"] = 10,
            ["РСЗВ"] = 5,
            ["Засоби ППО"] = 6,
            ["БПЛА"] = 13,
            ["Цистерни з ППМ"] = 12,
            ["Кораблі (катери)"] = 11,
            ["Спеціальна техніка"] = 14,
            ["Пускові установки ОТРК"] = 9,
            ["Особовий склад"] = 1
        };

        public List<RussinLoss> LastLosses(HtmlDocument doc)
        {
            var data = doc.DocumentNode.Descendants("li")
                .Where(node => node.GetAttributeValue("class", "").Contains("gold") && !node.GetAttributeValue("type", "").Contains("square"))
                .Select(s => new { date = s.FirstChild.InnerText, losses = s.SelectSingleNode("div").Descendants("li").Select(s => s.InnerText) })
                .Take(2)
                .ToDictionary(s => DateOnly.ParseExact(s.date, "dd.MM.yyyy"), s => s.losses.Select(k => GetLosses(k, "&nbsp;&mdash; ")).ToList());

            return ConvertToEntities(data)
                .Where(s => s.Date != DateOnly.FromDateTime(DateTime.Now.AddDays(-1)))
                .ToList();
        }

        public async Task<List<RussinLoss>> ParseFromFileAsync(string path)
        {
            using var file = File.OpenRead(path);
            using var reader = new StreamReader(file);

            var data = new Dictionary<DateOnly, List<(string name, int count)>>();
            var curentDate = default(DateOnly);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (DateOnly.TryParseExact(line, "dd.MM.yyyy", out var parsedDate))
                {
                    curentDate = parsedDate;
                    data.Add(curentDate, new List<(string name, int count)>());
                    continue;
                }

                data[curentDate].Add(GetLosses(line, " — "));
            }

            return ConvertToEntities(data)
                .ToList();
        }

        private (string name, int count) GetLosses(string line, string separator)
        {
            var rowItems = line.Split(separator);

            var secondValues = rowItems[1].Split(' ');
            int index = 0;
            int count;

            while (!int.TryParse(secondValues[index++], out count)) ;

            return (rowItems[0], count);
        }

        private IEnumerable<RussinLoss> ConvertToEntities(Dictionary<DateOnly, List<(string name, int count)>> data)
        {
            Dictionary<string, int>? preDay = default;
            return data
                .Reverse()
                .ToDictionary(s => s.Key, s => s.Value.ToDictionary(k => k.name, k => k.count))
                .Select(day =>
                {
                    if (preDay == default)
                    {
                        preDay = day.Value;
                        return day.Value.Select(s => new RussinLoss
                        {
                            Date = day.Key,
                            Count = s.Value,
                            LossTypeId = _nameToTypeMapper[s.Key]
                        }).ToList();
                    }

                    var next = day.Value.Select(s => new RussinLoss
                    {
                        Date = day.Key,
                        Count = preDay.TryGetValue(s.Key, out var value) ? s.Value - value : s.Value,
                        LossTypeId = _nameToTypeMapper[s.Key]
                    }).ToList();

                    preDay = day.Value;

                    return next;

                })
                .SelectMany(s => s);
        }
    }
}
