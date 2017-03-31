using System;
using System.Threading;
using System.Threading.Tasks;

namespace Carbon.Platform.Monitoring
{
    public class HostMonitor : IDisposable
    {
        private readonly HostObserver observable;
        private readonly Action<HostObserveration, HostObserveration> action;
        private readonly CancellationTokenSource ct = new CancellationTokenSource();

        private readonly Task task;

        private readonly TimeSpan interval;

        public HostMonitor(HostObserver observer, TimeSpan interval, Action<HostObserveration, HostObserveration> action)
        {
            this.observable = observer;
            this.interval = interval;
            this.action = action;

            task = Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning);
        }

        private async Task Run()
        {
            while (!ct.IsCancellationRequested)
            {
                Next();

                try
                {
                    await Task.Delay(interval, ct.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException) { }
            }
        }

        private HostObserveration last = null;

        public void Next()
        {
            try
            {
                var current = observable.Observe();

                if (last != null)
                {
                    action(last, current);
                }

                last = current;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error observing:" + ex.Message);
            }
        }

        public void Dispose()
        {
            ct.Cancel();

            task.Wait();
        }
    }
}