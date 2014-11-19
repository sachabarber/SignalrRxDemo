using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Hub.Transport
{
    public enum ConnectionStatus
    {
        Connecting,
        Connected,
        ConnectionSlow,
        Reconnecting,
        Reconnected,
        Closed,
        Uninitialized
    }
}
