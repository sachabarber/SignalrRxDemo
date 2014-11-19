using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ServiceConstants
    {
        public static class Server
        {
            public const string UsernameHeader = "User";

            public const string TickerHub = "TickerHub";
            public const string SubscribeTickers = "SubscribeTickers";
            public const string UnsubscribeTickers = "UnsubscribeTickers";

        }

        public static class Client
        {
            public const string SendTickers = "SendTickers";
            public const string ServerAddress = "http://localhost:5263/signalr";
        }
    }
}
