using System;
using System.Text.Json.Serialization;
using Model;
namespace Model
{
    public class Rooms
    {
        public RoomType? Room { get; set; }
        public int? Number { get; set; }
        public int? Size { get; set; }
        public int? Cost { get; set; }
        public List<AccountingUnit> Info { get; set; }

        [JsonConstructor]
        public Rooms()
        {
            Info = new List<AccountingUnit>();
        }
        public Rooms(RoomType room, int number, int size, int cost, List<AccountingUnit> info)
        {
            if (number < 0 || size < 0 || cost < 0)
                throw new ArgumentException("Invalid room data");

            Room = room;
            Number = number;
            Size = size;
            Cost = cost;
            Info = info;
        }
        public string AnimalNames
        {
            get
            {
                if (Info == null || Info.Count == 0)
                    return "";
                return string.Join(", ", Info.Select(a => a.Animal?.Name ?? "Без імені"));
            }
        }
        public void AddAnimal(AccountingUnit infos)
        {
            Info.Add(infos);
        }
        public string ToShortString()
        {
            int totalCost = 0;
            foreach (var unit in Info)
            {
                totalCost += unit.Price;
            }
            return $"Room #{Number}: Total Maintenance Cost = {totalCost}";
        }
        public override string ToString()
        {
            string animalNames = Info.Count == 0 ? "немає" : string.Join(", ", Info.Select(a => a.Animal.Name));

            return $"Кімната №{Number}({Room}) — Розмір: {Size}, Прибирання коштує: {Cost}₴, Тварини: {animalNames}";
        }
    }
}
