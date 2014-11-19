using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Client.Factory;
using Client.Services;
using Client.ViewModels;
using Client.ViewModels.MainWindow;

namespace Client.IOC
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();


            builder.RegisterType<ReactiveTrader>().As<IReactiveTrader>().SingleInstance();
            builder.RegisterType<UserProvider>().As<IUserProvider>();
            builder.RegisterType<ConcurrencyService>().As<IConcurrencyService>();


            // UI
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<MainWindowViewModel>().SingleInstance();
            builder.RegisterType<TickersViewModel>().SingleInstance();
            builder.RegisterType<ConnectivityStatusViewModel>().SingleInstance();
            builder.RegisterType<TickerViewModelFactory>().SingleInstance();


            return builder.Build();
        }
    }
}
