using System;
using System.Diagnostics;

using Carbon.Platform.Apps;

namespace Carbon.Platform.Monitoring
{
    public class AppObserver : IObservable<AppObservation>, IDisposable
    {
        private readonly IApp app;

        private readonly PerformanceCounter errorRate;
        private readonly PerformanceCounter totalRequests;
        private readonly PerformanceCounter totalBytesReceived;
        private readonly PerformanceCounter totalBytesSent;

        private bool ok = true;

        public AppObserver(IApp app)
        {
            this.app = app ?? throw new ArgumentNullException(nameof(app));

            // var site = (Site)this.appInstance.Site;

            var siteId = app.Id;
            var siteName = app.Name;

            var path = $"/LM/W3SVC/{siteId}/ROOT";

            var instanceName = path.Replace("/", "_");

            this.errorRate          = new PerformanceCounter("ASP.NET Applications", "Errors Total/Sec", instanceName);
            this.totalRequests      = new PerformanceCounter("Web Service", "Total Method Requests", siteName);
            this.totalBytesSent     = new PerformanceCounter("Web Service", "Total Bytes Sent", siteName);
            this.totalBytesReceived = new PerformanceCounter("Web Service", "Total Bytes Received", siteName);
        }

        public IApp App => app;

        public bool Faulted => !ok;

        public Exception Fault { get; set; }

        public AppObservation Observe()
        {
            try
            {
                var observation = new AppObservation(
                   appId              : 0,
                   totalRequests      : totalRequests.NextSample(),
                   totalBytesReceived : totalBytesReceived.NextSample(),
                   totalBytesSent     : totalBytesSent.NextSample(),
                   errorRateSample    : errorRate.NextSample(),
                   date               : DateTime.UtcNow
                );

                this.ok = true;

                return observation;
            }
            catch (Exception ex)
            {
                this.ok = false;
                this.Fault = ex;

                return null;
            }
        }

        public CounterSample TryGetSample(PerformanceCounter counter)
        {
            try
            {
                return counter.NextSample();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get counter '{counter.CounterName}' sample - {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            this.errorRate.Dispose();
            this.totalRequests.Dispose();
            this.totalBytesSent.Dispose();
            this.totalBytesReceived.Dispose();
        }
    }
}

// http://www.microsoft.com/technet/prodtechnol/WindowsServer2003/Library/IIS/ce298896-d98a-4b8d-9864-dc5d5a80e8a2.mspx?mfr=true

/*
Performance object

Performance counter

ASP.NET					Application Restarts
ASP.NET					Requests Queued
ASP.NET					Worker Process Restarts
ASP.NET	Applications	Errors Total
ASP.NET Applications	Requests/Sec
Processor				% CPU Utilization
*/
