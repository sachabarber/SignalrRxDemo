using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class ObservableExtensions
    {
        public static IObservable<T> LazilyConnect<T>(this IConnectableObservable<T> connectable, SingleAssignmentDisposable futureDisposable)
        {
            var connected = 0;
            return Observable.Create<T>(observer =>
            {
                var subscription = connectable.Subscribe(observer);
                if (Interlocked.CompareExchange(ref connected, 1, 0) == 0)
                {
                    if (!futureDisposable.IsDisposed)
                    {
                        futureDisposable.Disposable = connectable.Connect();
                    }
                }
                return subscription;
            }).AsObservable();
        }


        public static IObservable<TSource> TakeUntilInclusive<TSource>(this IObservable<TSource> source, Func<TSource, Boolean> predicate)
        {
            return Observable.Create<TSource>(
                observer => source.Subscribe(
                  item =>
                  {
                      observer.OnNext(item);
                      if (predicate(item))
                          observer.OnCompleted();
                  },
                  observer.OnError,
                  observer.OnCompleted
                )
              );
        }

        public static IObservable<T> CacheFirstResult<T>(this IObservable<T> observable)
        {
            // We are happy to lose the underlying subscription here because we have .Take(1) the source stream.
            return observable.Take(1).PublishLast().LazilyConnect(new SingleAssignmentDisposable());
        }


    }
}
