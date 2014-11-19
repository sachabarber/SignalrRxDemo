using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Hub.Transport;
using Client.Services;
using log4net;

namespace Client.ViewModels
{
    public class TickerViewModel : INPCBase
    {
        private decimal price;
        private bool isUp;
        private bool stale;
        private bool disconnected;
        private static readonly ILog log = LogManager.GetLogger(typeof(TickerViewModel));


        public TickerViewModel(
            IReactiveTrader reactiveTrader,
            IConcurrencyService concurrencyService,
            string name)
        {

            this.Name = name;

            reactiveTrader.ConnectionStatusStream
                .ObserveOn(concurrencyService.Dispatcher)
                .SubscribeOn(concurrencyService.TaskPool)
                .Subscribe(
                    OnStatusChange,
                    ex => log.Error("An error occurred within the connection status stream.", ex));
        }


        public string Name { get; private set; }


        public void AcceptNewPrice(decimal newPrice)
        {
            IsUp = newPrice > price;
            Price = newPrice;
        }


        public decimal Price
        {
            get { return this.price; }
            private set
            {
                this.price = value;
                base.OnPropertyChanged("Price");
            }
        }

        public bool IsUp
        {
            get { return this.isUp; }
            private set
            {
                this.isUp = value;
                base.OnPropertyChanged("IsUp");
            }
        }

        public bool Stale
        {
            get { return this.stale; }
            set
            {
                this.stale = value;
                base.OnPropertyChanged("Stale");
            }
        } 
        
        public bool Disconnected
        {
            get { return this.disconnected; }
            set
            {
                this.disconnected = value;
                base.OnPropertyChanged("Disconnected");
            }
        }


        private void OnStatusChange(ConnectionInfo connectionInfo)
        {

            switch (connectionInfo.ConnectionStatus)
            {
                case ConnectionStatus.Uninitialized:
                case ConnectionStatus.Connecting:
                    Disconnected = true;
                    break;
                case ConnectionStatus.Reconnected:
                case ConnectionStatus.Connected:
                    Disconnected = false;
                    break;
                case ConnectionStatus.ConnectionSlow:
                    Disconnected = false;
                    break;
                case ConnectionStatus.Reconnecting:
                    Disconnected = true;
                    break;
                case ConnectionStatus.Closed:
                    Disconnected = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
