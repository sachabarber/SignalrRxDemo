using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Hub.Transport;
using Client.Repositories;
using log4net.Repository.Hierarchy;

namespace Client.Services
{
    public interface IReactiveTrader
    {
        ITickerRepository TickerRepository { get; }
        IObservable<ConnectionInfo> ConnectionStatusStream { get; }
        void Initialize(string username, string server, string authToken = null);
    }
}
