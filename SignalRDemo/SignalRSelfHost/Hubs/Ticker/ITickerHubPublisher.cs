using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SignalRSelfHost.Hubs.Ticker
{
    public interface ITickerHubPublisher
    {
        void Start();
        void Stop();
        Task SendOneManualFakeTicker();
    }
}
