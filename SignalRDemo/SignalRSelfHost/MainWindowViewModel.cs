using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.ViewModels;
using log4net;
using Microsoft.Owin.BuilderProperties;
using Microsoft.Owin.Hosting;
using SignalRSelfHost.Hubs.Ticker;

namespace SignalRSelfHost
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        private const string Address = "http://localhost:5263";
        
        private readonly ITickerHubPublisher tickerHubPublisher;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindowViewModel));
        private IDisposable signalr;

        public MainWindowViewModel(ITickerHubPublisher tickerHubPublisher)
        {
            this.tickerHubPublisher = tickerHubPublisher;

            AutoTickerStartCommand = new DelegateCommand(tickerHubPublisher.Start);
            AutoTickerStopCommand = new DelegateCommand(tickerHubPublisher.Stop);
            SendOneTickerCommand = new DelegateCommand(async () =>
            {
                await tickerHubPublisher.SendOneManualFakeTicker();
            });
            StartCommand = new DelegateCommand(StartServer);
            StopCommand = new DelegateCommand(StopServer);
        }

        public ICommand AutoTickerStartCommand { get; set; }
        public ICommand AutoTickerStopCommand { get; set; }
        public ICommand SendOneTickerCommand { get; set; }
        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }

        public void Start()
        {
            StartServer();
        }
       



        private void StartServer()
        {

            try
            {
                signalr = WebApp.Start(Address);
                tickerHubPublisher.Start();
            }
            catch (Exception exception)
            {
                Log.Error("An error occurred while starting SignalR", exception);
            }
        }


        private void StopServer()
        {
            if (signalr != null)
            {
                tickerHubPublisher.Stop();
                signalr.Dispose();
                signalr = null;
            }

        }

    }
}
