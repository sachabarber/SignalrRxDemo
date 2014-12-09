using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Client.Factory;
using Client.Hub;

namespace Client.Repositories
{
    class TickerRepository : ITickerRepository
    {
        private readonly ITickerHubClient tickerHubClient;
        private readonly ITickerFactory tickerFactory;

        public TickerRepository(ITickerHubClient tickerHubClient, ITickerFactory tickerFactory)
        {
            this.tickerHubClient = tickerHubClient;
            this.tickerFactory = tickerFactory;
        }

        public IObservable<IEnumerable<Ticker>> GetTickerStream()
        {
            return Observable.Defer(() => tickerHubClient.GetTickerStream())
                .Select(trades => trades.Select(tickerFactory.Create))
                .Catch(Observable.Return(new Ticker[0]))
                .Repeat()
                .Publish()
                .RefCount();
        }
    }
}
