using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private CancellationTokenSource autoRunningCancellationToken;
        private Task autoRunningTask;


        public TickerHubPublisher(IContextHolder contextHolder,
            ITickerRepository tickerRepository)
        {
            this.contextHolder = contextHolder;
            this.tickerRepository = tickerRepository;
        }


        public void Stop()
        {
            if (autoRunningCancellationToken != null)
            {
                autoRunningCancellationToken.Cancel();

                // Publisher is not thread safe, so while the auto ticker is running only the autoticker is 
                // allowed to access the publisher. Therefore before we can stop the publisher we have to 
                // wait for the autoticker task to complete
                autoRunningTask.Wait();

                autoRunningCancellationToken = null;
            }
        }

        public async void Start()
        {
            autoRunningCancellationToken = new CancellationTokenSource();
            autoRunningTask = Task.Run(async () =>
            {
                while (!autoRunningCancellationToken.IsCancellationRequested)
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
    }
}
