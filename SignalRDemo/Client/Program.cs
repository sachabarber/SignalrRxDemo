//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Common;
//using Microsoft.AspNet.SignalR.Client;

//namespace Client
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Program p = new Program();
//            p.StartHub();
//            Console.ReadLine();
//        }


//        public async void StartHub()
//        {
//            var hubConnection = new HubConnection("http://localhost:5263/signalr");
//            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("TickerHub");

//            stockTickerHubProxy.On<IEnumerable<TickerInfo>>("SendTickers", tickers =>
//            {
//                foreach (var ticker in tickers)
//                {
//                    Console.WriteLine("Stock update for '{0}' new price {1}", ticker.Name, ticker.Price);
//                }

//            });
//            await hubConnection.Start();
//        }
//    }
//}
