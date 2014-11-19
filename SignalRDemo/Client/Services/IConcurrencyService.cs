using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IConcurrencyService 
    {

         IScheduler Dispatcher { get; }


         IScheduler TaskPool { get; }

    }
}
