using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Services;

namespace Client.ViewModels
{
    public class TickerViewModelFactory
    {
        private readonly IReactiveTrader reactiveTrader;
        private readonly IConcurrencyService concurrencyService;


        public TickerViewModelFactory(
            IReactiveTrader reactiveTrader,
            IConcurrencyService concurrencyService)
        {
            this.reactiveTrader = reactiveTrader;
            this.concurrencyService = concurrencyService;
        }

        public TickerViewModel Create(string name)
        {
            return new TickerViewModel(reactiveTrader, concurrencyService, name);
        }
    }
}
