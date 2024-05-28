using System;

namespace GrupoLTM.WebSmart.Infrastructure.Helpers
{
    public static class StopWatchHelper
    {
        public static  StopWatchData<T> Invoke<T>(Func<T> func)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = func.Invoke();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return new StopWatchData<T>
            {
                Data = result,
                ElapsedMilliseconds = elapsedMs
            };
        }
        public static StopWatchData<int> Invoke(Action action)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            action.Invoke();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return new StopWatchData<int>
            {
                Data = default(int),
                ElapsedMilliseconds = elapsedMs
            };
        }
        public class StopWatchData<T>
        {
            public T Data { get; set; }
            public long ElapsedMilliseconds { get; set; }
        }
    }
}
