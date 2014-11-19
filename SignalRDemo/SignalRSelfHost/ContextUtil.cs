using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRSelfHost
{
    public static class ContextUtil
    {
        public static string GetUserName(HubCallerContext context)
        {
            var userName = context.Headers[ServiceConstants.Server.UsernameHeader];
            if (string.IsNullOrEmpty(userName))
            {
                return context.QueryString[ServiceConstants.Server.UsernameHeader];
            }

            return userName;
        }
    }
}
