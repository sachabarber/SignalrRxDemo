using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SignalRSelfHost.Hubs.Ticker
{
   public class TickerRepository : ITickerRepository
    {
       private readonly Queue<TickerDto> tickers = new Queue<TickerDto>();
       private object syncLock = new object();
       private const int MaxTrades = 50;


       public TickerRepository()
       {
           tickers.Enqueue(new TickerDto() {Name="Yahoo", Price=1.2m});
           tickers.Enqueue(new TickerDto() {Name="Google", Price=1022m});
           tickers.Enqueue(new TickerDto() {Name="Apple", Price=523m});
           tickers.Enqueue(new TickerDto() {Name="Facebook", Price=49m});
           tickers.Enqueue(new TickerDto() {Name="Microsoft", Price=37m});
           tickers.Enqueue(new TickerDto() {Name="Twitter", Price=120m});
       }



       public TickerDto GetNextTicker()
       {
           return tickers.Dequeue();
       }


        public void StoreTicker(TickerDto tickerInfo)
        {
            lock (syncLock)
            {
                tickers.Enqueue(tickerInfo);

                if (tickers.Count > MaxTrades)
                {
                    tickers.Dequeue();
                }
            }
        }

        public IList<TickerDto> GetAllTickers()
        {
            IList<TickerDto> newTickers;

            lock (syncLock)
            {
                newTickers = tickers.ToList();
            }

            return newTickers;
        } 
    }
}

