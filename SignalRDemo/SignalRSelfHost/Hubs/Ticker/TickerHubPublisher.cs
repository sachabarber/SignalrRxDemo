using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Common;
using log4net;
using Microsoft.AspNet.SignalR;

namespace SignalRSelfHost.Hubs.Ticker
{
    public class TickerHubPublisher : ITickerHubPublisher
    {
        private readonly IContextHolder contextHolder;
        private readonly ITickerRepository tickerRepository;
        Random rand = new Random();
        private static readonly ILog Log = LogManager.GetLogger(typeof(TickerHubPublisher));
        private bool tickerAutoRunning = false;


        public TickerHubPublisher(IContextHolder contextHolder,
            ITickerRepository tickerRepository)
        {
            this.contextHolder = contextHolder;
            this.tickerRepository = tickerRepository;
        }


        public void Stop()
        {
            tickerAutoRunning = false;
        }

        public async void Start()
        {
            tickerAutoRunning = true;
            await Task.Run(async () =>
            {
                while (tickerAutoRunning)
                {
                    await SendOneManualFakeTicker();


                    await Task.Delay(20);
                }
            });


        }


        public async Task SendOneManualFakeTicker()
        {
            var currentTicker = tickerRepository.GetNextTicker();


            var flipPoint = rand.Next(0, 100);

            if (flipPoint > 50)
            {
                currentTicker.Price += currentTicker.Price/ 30;
            }
            else
            {
                currentTicker.Price -= currentTicker.Price / 30;
            }


            tickerRepository.StoreTicker(currentTicker);
            await SendRandomTicker(currentTicker);
        }


        //to need to do subscribe stuff
        private Task SendRandomTicker(TickerDto tickerInfo)
        {
            if (contextHolder.TickerHubClients == null) return Task.FromResult(false);

            Log.InfoFormat("Broadcast new trade to blotters: {0}", tickerInfo);
            return contextHolder.TickerHubClients.Group(TickerHub.TickerGroupName).SendTickers(new[] { tickerInfo });
        }

        private Task SendRandomTickerAll(TickerDto tickerInfo)
        {
            if (contextHolder.TickerHubClients == null) return Task.FromResult(false);

            Log.InfoFormat("Broadcast new trade to blotters: {0}", tickerInfo);



            return contextHolder.TickerHubClients.Group(TickerHub.TickerGroupName).SendTickers(new[] { tickerInfo });
        }

        
    }
}
