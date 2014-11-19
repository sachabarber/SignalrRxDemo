using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Hub.Transport
{
    public class ConnectionInfo
    {
        public ConnectionStatus ConnectionStatus { get; private set; }
        public string Server { get; private set; }

        public ConnectionInfo(ConnectionStatus connectionStatus, string server)
        {
            ConnectionStatus = connectionStatus;
            Server = server;
        }

        public override string ToString()
        {
            return string.Format("ConnectionStatus: {0}, Server: {1}", ConnectionStatus, Server);
        }
    }
}
