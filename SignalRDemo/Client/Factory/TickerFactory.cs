using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client.Factory
{
    internal class TickerFactory : ITickerFactory
    {
        public Ticker Create(TickerDto ticker)
        {
            return new Ticker(
                ticker.Name,
                ticker.Price);
        }
    }
}
