using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Model
{
    [Serializable]
    public class AccountingUnit
    {
        public string Name { get; set; }
        public Animal Animal { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }

        public AccountingUnit(Animal animal, DateTime date, int price)
        {
            if (price < 0)
                throw new ArgumentException("Price must be >= 0.");
            if (date > DateTime.Now)
                throw new ArgumentException("Date of arrival can't be in the future.");

            Name= animal.Name;
            Animal = animal;
            Date = date;
            Price = price;
        }
        public override string ToString() => $"{Animal} — {Price}₴ on {Date}";
    }
}
