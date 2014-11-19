using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Factory;
using Client.Hub;
using Client.Hub.Transport;
using Client.Repositories;
using log4net;

namespace Client.Services
{
    public class ReactiveTrader : IReactiveTrader, IDisposable
    {
        private ConnectionProvider _connectionProvider;
        private static readonly ILog log = LogManager.GetLogger(typeof(ReactiveTrader));

        public void Initialize(string username, string server, string authToken = null)
        {
            _connectionProvider = new ConnectionProvider(username, server);

            var tickerHubClient = new TickerHubClient(_connectionProvider);

            //if (authToken != null)
            //{
            //    var controlServiceClient = new ControlServiceClient(new AuthTokenProvider(authToken), _connectionProvider, _loggerFactory);
            //    _controlRepository = new ControlRepository(controlServiceClient);
            //}

            var concurrencyService = new ConcurrencyService();

            var tickerFactory = new TickerFactory();
            TickerRepository = new TickerRepository(tickerHubClient, tickerFactory);
        }

        public ITickerRepository TickerRepository { get; private set; }


        public IObservable<ConnectionInfo> ConnectionStatusStream
        {
            get
            {
                return _connectionProvider.GetActiveConnection()
                    .Do(_ => log.Info("New connection created by connection provider"))
                    .Select(c => c.StatusStream)
                    .Switch()
                    .Publish()
                    .RefCount();
            }
        }

        public void Dispose()
        {
            _connectionProvider.Dispose();
        }
    }

}
