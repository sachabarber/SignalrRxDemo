using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRSelfHost.Hubs
{
    public class ContextHolder : IContextHolder
    {
        public IHubCallerConnectionContext<dynamic> TickerHubClients { get; set; }
    }
}
