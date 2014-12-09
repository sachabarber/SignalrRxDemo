using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Factory;
using Client.Repositories;
using Client.Services;
using Common;
using Common.ViewModels;
using log4net;


namespace Client.ViewModels
{
    public class TickersViewModel : INPCBase
    {
        private readonly ITickerRepository tickerRepository;
        private readonly IConcurrencyService concurrencyService;
        private bool stale = false;
        private static readonly ILog log = LogManager.GetLogger(typeof(TickersViewModel));

        public TickersViewModel(IReactiveTrader reactiveTrader,
                                IConcurrencyService concurrencyService,
            TickerViewModelFactory tickerViewModelFactory)
        {
            Tickers = new ObservableCollection<TickerViewModel>();
            Tickers.Add(tickerViewModelFactory.Create("Yahoo"));
            Tickers.Add(tickerViewModelFactory.Create("Google"));
            Tickers.Add(tickerViewModelFactory.Create("Apple"));
            Tickers.Add(tickerViewModelFactory.Create("Facebook"));
            Tickers.Add(tickerViewModelFactory.Create("Microsoft"));
            Tickers.Add(tickerViewModelFactory.Create("Twitter"));
            this.tickerRepository = reactiveTrader.TickerRepository;
            this.concurrencyService = concurrencyService;
            LoadTrades();


           
        }


        public ObservableCollection<TickerViewModel> Tickers { get; private set; }

        private void LoadTrades()
        {
            tickerRepository.GetTickerStream()
                            .ObserveOn(concurrencyService.Dispatcher)
                            .SubscribeOn(concurrencyService.TaskPool)
                            .Subscribe(
                                AddTickers,
                                ex => log.Error("An error occurred within the trade stream", ex));
        }

        private void AddTickers(IEnumerable<Ticker> incomingTickers)
        {
            var allTickers = incomingTickers as IList<Ticker> ?? incomingTickers.ToList();
            if (!allTickers.Any())
            {
                // empty list of trades means we are disconnected
                stale = true;
            }
            else
            {
                if (stale)
                {
                    stale = false;
                }
            }

            foreach (var ticker in allTickers)
            {
                Tickers.Single(x => x.Name == ticker.Name)
                    .AcceptNewPrice(ticker.Price);
            }
        }
    }
}
