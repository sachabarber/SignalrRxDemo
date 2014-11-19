using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Factory;
using Client.Repositories;
using Client.Services;
using Common;
using Common.ViewModels;
using log4net;


namespace Client.ViewModels.MainWindow
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(
            TickersViewModel tickersViewModel,
            ConnectivityStatusViewModel connectivityStatusViewModel
            
            )
        {
            this.TickersViewModel = tickersViewModel;
            this.ConnectivityStatusViewModel = connectivityStatusViewModel;
        }


        public TickersViewModel TickersViewModel { get; private set; }
        public ConnectivityStatusViewModel ConnectivityStatusViewModel { get; private set; }
    }
}
