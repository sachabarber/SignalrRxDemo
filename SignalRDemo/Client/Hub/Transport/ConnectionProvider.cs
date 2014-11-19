using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Common.Extensions;


namespace Client.Hub.Transport
{
    /// <summary>
    /// Connection provider provides always the same connection until it fails then create a new one a yield it
    /// Connection provider randomizes the list of server specified in configuration and then round robin through the list
    /// </summary>
    internal class ConnectionProvider : IConnectionProvider, IDisposable
    {
        private readonly SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();
        private readonly string username;
        private readonly IObservable<IConnection> connectionSequence;
        private readonly string server;
        private int _currentIndex;
        private static readonly ILog log = LogManager.GetLogger(typeof(ConnectionProvider));

        public ConnectionProvider(string username, string server)
        {
            this.username = username;
            this.server = server;
            connectionSequence = CreateConnectionSequence();
        }

        public IObservable<IConnection> GetActiveConnection()
        {
            return connectionSequence;
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        private IObservable<IConnection> CreateConnectionSequence()
        {
            return Observable.Create<IConnection>(o =>
            {
                log.Info("Creating new connection...");
                var connection = GetNextConnection();

                var statusSubscription = connection.StatusStream.Subscribe(
                    _ => { },
                    ex => o.OnCompleted(),
                    () =>
                    {
                        log.Info("Status subscription completed");
                        o.OnCompleted();
                    });

                var connectionSubscription =
                    connection.Initialize().Subscribe(
                        _ => o.OnNext(connection),
                        ex => o.OnCompleted(),
                        o.OnCompleted);

                return new CompositeDisposable { statusSubscription, connectionSubscription };
            })
                .Repeat()
                .Replay(1)
                .LazilyConnect(disposable);
        }

        private IConnection GetNextConnection()
        {
            return new Client.Hub.Transport.Connection(server, username);
        }
    }

}
