using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Hub.Transport;
using Common;
using log4net;
using Microsoft.AspNet.SignalR.Client;

namespace Client.Hub
{
    internal class TickerHubClient : ServiceClientBase, ITickerHubClient
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(TickerHubClient));

        public TickerHubClient(IConnectionProvider connectionProvider)
            : base(connectionProvider)
        {
        }

        public IObservable<IEnumerable<TickerDto>> GetTickerStream()
        {
            return GetResilientStream(connection => GetTradesForConnection(connection.TickerHubProxy), 
                TimeSpan.FromSeconds(5));
        }

        private IObservable<IEnumerable<TickerDto>> GetTradesForConnection(IHubProxy tickerHubProxy)
        {
            return Observable.Create<IEnumerable<TickerDto>>(observer =>
            {
                // subscribe to trade feed first, otherwise there is a race condition 
                var spotTradeSubscription = tickerHubProxy.On<IEnumerable<TickerDto>>(
                    ServiceConstants.Client.SendTickers, observer.OnNext);

                var spotTradeSubscriptionRaceDisposable = 
                    tickerHubProxy.On<IEnumerable<TickerDto>>(
                    ServiceConstants.Client.SendTickers, 
                    (x) => 
                    {
                            Console.WriteLine("Got a new trade" + x.First().Name);
                    });



                log.Info("Sending ticker subscription...");
                var sendSubscriptionDisposable = SendSubscription(tickerHubProxy)
                    .Subscribe(
                        _ => log.InfoFormat("Subscribed to ticker."),
                        observer.OnError);

                var unsubscriptionDisposable = Disposable.Create(() =>
                {
                    // send unsubscription when the observable gets disposed
                    log.Info("Sending ticker unsubscription...");
                    SendUnsubscription(tickerHubProxy)
                        .Subscribe(
                            _ => log.InfoFormat("Unsubscribed from ticker."),
                            ex => log.WarnFormat("An error occurred while unsubscribing from ticker: {0}", 
                                ex.Message));
                });
                return new CompositeDisposable { 
                    spotTradeSubscription, unsubscriptionDisposable, 
                    sendSubscriptionDisposable,spotTradeSubscriptionRaceDisposable 
                };
            })
            .Publish()
            .RefCount();
        }

        private static IObservable<Unit> SendSubscription(IHubProxy tickerHubProxy)
        {
            return Observable.FromAsync(() => tickerHubProxy.Invoke(
                ServiceConstants.Server.SubscribeTickers));
        }

        private static IObservable<Unit> SendUnsubscription(IHubProxy tickerrHubProxy)
        {
            return Observable.FromAsync(() => tickerrHubProxy.Invoke(
                ServiceConstants.Server.UnsubscribeTickers));

        }
    }
}
