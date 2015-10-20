namespace Carbon.Platform
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class AsyncLock
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly Task<IDisposable> releaser;

        public AsyncLock()
        {
            releaser = Task.FromResult((IDisposable)new Releaser(this));
        }

        public bool IsLocked => semaphore.CurrentCount > 0;

        public Task<IDisposable> LockAsync()
        {
            var wait = semaphore.WaitAsync();

            return wait.IsCompleted ?
                releaser : wait.ContinueWith(
                    (_, state) => (IDisposable)state,
                    releaser.Result,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default
                );
        }

        public Task<IDisposable> LockAsync(TimeSpan timeout)
        {
            var wait = semaphore.WaitAsync(timeout);

            return wait.IsCompleted ?
                releaser : wait.ContinueWith(
                    (_, state) => (IDisposable)state,
                    releaser.Result,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default
                );

        }

        private sealed class Releaser : IDisposable
        {
            private readonly AsyncLock toRelease;

            internal Releaser(AsyncLock toRelease)
            {
                this.toRelease = toRelease;
            }
            public void Dispose()
            {
                toRelease.semaphore.Release();
            }
        }
    }
}
