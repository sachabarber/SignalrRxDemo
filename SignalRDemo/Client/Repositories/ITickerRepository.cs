using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Factory;
using Common;

namespace Client.Repositories
{
    public interface ITickerRepository
    {
        IObservable<IEnumerable<Ticker>> GetTickerStream();
    }
}
