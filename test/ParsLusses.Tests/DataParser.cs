using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParsLusses.Tests
{
    /// <summary>
    /// Data source https://www.mil.gov.ua/
    /// </summary>
    internal class DataParser
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

        public async Task<List<RussinLoss>> ParsFromFileAsync(string path)
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

                data[curentDate].Add(GetLosses(line));
            }

            return ConvertToEntities(data).ToList();
        }

        private (string name, int count) GetLosses(string line)
        {
            var rowItems = line.Split(" — ");

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
