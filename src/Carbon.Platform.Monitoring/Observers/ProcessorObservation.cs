using System;
using System.Diagnostics;

namespace Carbon.Platform.Monitoring
{
    public class ProcessorObservation
    {
        public ProcessorObservation(CounterSample systemTimeSample, CounterSample userTimeSample, DateTime date)
        {
            SystemTimeSample = systemTimeSample;
            UserTimeSample   = userTimeSample;
            Date             = date;
        }

        public CounterSample SystemTimeSample { get; }

        public CounterSample UserTimeSample { get; }

        public DateTime Date { get; }
    }
}