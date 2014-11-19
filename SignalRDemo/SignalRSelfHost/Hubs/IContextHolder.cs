using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRSelfHost.Hubs
{
    public interface IContextHolder
    {
        IHubCallerConnectionContext<dynamic> TickerHubClients { get; set; }
    }
}
