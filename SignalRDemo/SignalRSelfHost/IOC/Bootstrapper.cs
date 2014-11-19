using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using SignalRSelfHost.Hubs;
using SignalRSelfHost.Hubs.Ticker;

namespace SignalRSelfHost.IOC
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

          

            // ticker
            builder.RegisterType<TickerHubPublisher>().As<ITickerHubPublisher>().SingleInstance();
            builder.RegisterType<TickerHub>().SingleInstance();
            builder.RegisterType<ContextHolder>().As<IContextHolder>().SingleInstance();



           builder.RegisterType<TickerRepository>().As<ITickerRepository>().SingleInstance();

            // UI
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<MainWindowViewModel>().SingleInstance();

            return builder.Build();
        }
    }
}
