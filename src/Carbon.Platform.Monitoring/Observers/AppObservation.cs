using System;
using System.Diagnostics;

namespace Carbon.Platform.Monitoring
{
    public class AppObservation
    {
        public AppObservation(
            long appId,
            CounterSample totalRequests,
            CounterSample totalBytesReceived,
            CounterSample totalBytesSent,
            CounterSample errorRateSample, 
            DateTime date
        )
        {
            AppId = appId;
            TotalRequestsSample = totalRequests;
            TotalBytesReceived = totalBytesReceived;
            TotalBytesSent = totalBytesSent;
            ErrorRateSample = errorRateSample;
        }

        public long AppId { get; set; }

        public CounterSample TotalRequestsSample { get; set; }

        public CounterSample TotalBytesReceived { get; set; }

        public CounterSample TotalBytesSent { get; set; }

        public CounterSample ErrorRateSample { get; set; }

        public DateTime Date { get; set; }

    }
}