using System;
using System.Diagnostics;

namespace Carbon.Platform.Monitoring
{
    public class ProcessorObserver : IObservable<ProcessorObservation>, IDisposable
    {
        private readonly PerformanceCounter userTimeCounter;
        private readonly PerformanceCounter systemTimeCounter;

        // Name is 0 indexed...

        public ProcessorObserver(string processorName = "_Total")
        {
            #region Preconditions

            if (processorName == null) throw new ArgumentNullException(nameof(processorName));

            #endregion

            this.userTimeCounter   = new PerformanceCounter("Processor", "% User Time", processorName);
            this.systemTimeCounter = new PerformanceCounter("Processor", "% Privileged Time", processorName);
        }

        // https://technet.microsoft.com/en-us/library/cc938593.aspx

        public ProcessorObservation Observe()
        {
            return new ProcessorObservation(
                systemTimeSample : systemTimeCounter.NextSample(),
                userTimeSample   : userTimeCounter.NextSample(),
                date             : DateTime.UtcNow);
        }

        public void Dispose()
        {
            userTimeCounter.Dispose();
            systemTimeCounter.Dispose();
        }
    }
}
