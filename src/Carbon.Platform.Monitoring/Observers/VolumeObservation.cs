using System;
using System.Diagnostics;

namespace Carbon.Platform.Monitoring
{
    public class VolumeObservation
    {
        public VolumeObservation(
            long size,
            long available, 
            CounterSample readTimeSample, 
            CounterSample writeTimeSample, 
            CounterSample readRateSample,
            CounterSample writeRateSample,
            DateTime date)
        {
            Size            = size;
            Available       = available;
            ReadTimeSample  = readTimeSample;
            WriteTimeSample = writeTimeSample;
            ReadRateSample  = readRateSample;
            WriteRateSample = writeRateSample;
            Date            = date;
        }

        public long Available { get; }

        public long Size { get; }

        public CounterSample ReadTimeSample { get; }

        public CounterSample WriteTimeSample { get; }

        public CounterSample ReadRateSample { get; }

        public CounterSample WriteRateSample { get; }

        public DateTime Date { get; }
    }
}