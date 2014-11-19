using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SignalRSelfHost.Hubs.Ticker
{
    public interface ITickerRepository
    {
        TickerDto GetNextTicker();
        void StoreTicker(TickerDto tickerInfo);
        IList<TickerDto> GetAllTickers();
    }
}
