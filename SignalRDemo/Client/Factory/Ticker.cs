using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Factory
{
    public class Ticker
    {
        public Ticker(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Price: {1}", Name, Price);
        }
    }
}
