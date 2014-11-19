using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Hub.Transport
{
    internal interface IConnectionProvider
    {
        IObservable<IConnection> GetActiveConnection();
    }
}
