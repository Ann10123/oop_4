using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Model
{
    [Serializable]
    public class Animal
    {
        public string View { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime Birthday { get; set; }

        public Animal(string view, string name, string country, DateTime birthday)
        {
            if (string.IsNullOrWhiteSpace(view) || string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("View and Name must not be empty.");
            if (birthday > DateTime.Now)
                throw new ArgumentException("Birthday can't be in the future."); 

            View = view;
            Name = name;
            Country = country;
            Birthday = birthday;
        }
        public override string ToString() => $"{Name} ({View}, {Country})";
    }
}
