using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using Autofac;
using Client.IOC;
using Client.Services;
using Client.ViewModels.MainWindow;
using Common;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.AspNet.SignalR.Client;


namespace Client
{
 
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeLogging();
            Start();
        }

        private void Start()
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Build();

            Log.Info("Initializing reactive trader API...");
            var reactiveTraderApi = container.Resolve<IReactiveTrader>();

            var username = container.Resolve<IUserProvider>().Username;
            reactiveTraderApi.Initialize(username, Common.ServiceConstants.Client.ServerAddress);

            var mainWindow = container.Resolve<MainWindow>();
            //var vm = container.Resolve<MainWindowViewModel>();
            //mainWindow.DataContext = vm;
            mainWindow.Show();


            //StartHub();

        }


        private void InitializeLogging()
        {
            Thread.CurrentThread.Name = "UI";
            log4net.Config.XmlConfigurator.Configure();
            Log.Info(@"SignalRSelfHost started");
        }



        public async void StartHub()
        {


            var hubConnection = new HubConnection("http://localhost:5263/signalr");
            IHubProxy tickerHubProxy = hubConnection.CreateHubProxy("TickerHub");

            await hubConnection.Start();
            SendSubscription(tickerHubProxy).Wait();



            tickerHubProxy.On<IEnumerable<TickerDto>>("SendTickers", tickers =>
            {
                foreach (var ticker in tickers)
                {
                    Console.WriteLine("Stock update for '{0}' new price {1}", ticker.Name, ticker.Price);
                }

            });
            
        }


        private static IObservable<Unit> SendSubscription(IHubProxy tickerHubProxy)
        {
            return Observable.FromAsync(() => tickerHubProxy.Invoke(ServiceConstants.Server.SubscribeTickers));
        }
            
    }
}
