using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRSelfHost.Hubs.Ticker
{
    [HubName(ServiceConstants.Server.TickerHub)]
    public class TickerHub : Hub
    {
        private readonly ITickerRepository tickerRepository;
        private readonly IContextHolder contextHolder;

        public const string TickerGroupName = "AllTickers";
        private static readonly ILog Log = LogManager.GetLogger(typeof(TickerHub));

        public TickerHub(ITickerRepository tickerRepository, IContextHolder contextHolder)
        {
            this.tickerRepository = tickerRepository;
            this.contextHolder = contextHolder;
        }

        [HubMethodName(ServiceConstants.Server.SubscribeTickers)]
        public async Task SubscribeTrades()
        {

            contextHolder.TickerHubClients = Clients;

            var user = ContextUtil.GetUserName(Context);
            Log.InfoFormat("Received trade subscription from user {0}", user);

            // add client to the trade notification group
            await Groups.Add(Context.ConnectionId, TickerGroupName);
            Log.InfoFormat("Connection {0} of user {1} added to group '{2}'", Context.ConnectionId, user, TickerGroupName);

            var tickers = tickerRepository.GetAllTickers();
            await Clients.Caller.SendTickers(tickers);
            Log.InfoFormat("Snapshot published to {0}", Context.ConnectionId);
        }

        [HubMethodName(ServiceConstants.Server.UnsubscribeTickers)]
        public async Task UnsubscribeTrades()
        {
            Log.InfoFormat("Received unsubscription request for trades from connection {0}", Context.ConnectionId);

            // remove client from the blotter group
            await Groups.Remove(Context.ConnectionId, TickerGroupName);
            Log.InfoFormat("Connection {0} removed from group '{1}'", Context.ConnectionId, TickerGroupName);
        }
    }
}

