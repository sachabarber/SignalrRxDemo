using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Client.Hub.Transport
{
    internal interface IConnection
    {
        IObservable<ConnectionInfo> StatusStream { get; }
        IObservable<Unit> Initialize();
        string Address { get; }
        void SetAuthToken(string authToken);
        IHubProxy TickerHubProxy { get; }

    }
}
