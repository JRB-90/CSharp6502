using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace CS6502.UIConsole.Shared
{
    public static class ObservableExtensions
    {
        public static IObservable<List<T>> BackBuffer<T>(this IObservable<T> source, TimeSpan ts)
        {
            return BackBuffer(source, ts, Scheduler.Default);
        }

        public static IObservable<List<T>> BackBuffer<T>(this IObservable<T> source, TimeSpan ts, IScheduler scheduler)
        {
            return source
                .Timestamp()
                .Scan(new List<Timestamped<T>>(), (list, element) => list
                    .Where(ti => scheduler.Now - ti.Timestamp <= ts)
                    .Concat(Enumerable.Repeat(element, 1))
                    .ToList()
                )
                .Select(list => list.Select(t => t.Value).ToList());
        }
    }
}
