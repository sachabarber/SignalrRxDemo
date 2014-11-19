using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client.Hub
{
    internal interface ITickerHubClient
    {
        IObservable<IEnumerable<TickerDto>> GetTickerStream();
    }
}
