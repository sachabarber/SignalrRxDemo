using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public sealed class ConcurrencyService : IConcurrencyService
    {

        public IScheduler Dispatcher
        {
            get { return DispatcherScheduler.Current; }
        }

        public IScheduler TaskPool
        {
            get { return ThreadPoolScheduler.Instance; }
        }

    }
}
