﻿using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scraper.Net.Stream
{
    public static class ObservableExtensions
    {
        public static IObservable<TSource> WhereAwait<TSource>(
            this IObservable<TSource> source,
            Func<TSource, Task<bool>> predicate)
        {
            return source
                .SelectMany(
                    taskSelector: predicate,
                    resultSelector: (v, p) => (v, p))
                .Where(x => x.p)
                .Select(x => x.v);
        }
        
        public static IDisposable SubscribeAsync<T>(
            this IObservable<T> source,
            Func<CancellationToken, Task> onNextAsync,
            IScheduler scheduler)
        {
            return source
                .Select(_ => Observable.FromAsync(onNextAsync, scheduler))
                .Concat()
                .Subscribe();
        }
    }
}