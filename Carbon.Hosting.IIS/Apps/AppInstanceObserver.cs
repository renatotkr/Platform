namespace Carbon.Platform.Hosting
{
	using System;
	using System.Diagnostics;
	using System.IO;

	using Microsoft.Web.Administration;
	using System.Collections.Generic;

	public class AppInstanceObserver : IDisposable
	{
		private readonly AppInstance appInstance;

		private readonly PerformanceCounter requestRate;
		private readonly PerformanceCounter errorRate;
		private readonly PerformanceCounter sendRate;
		private readonly PerformanceCounter recieveRate;
		private readonly PerformanceCounter totalRequests;
		private readonly PerformanceCounter uptime;

		private readonly List<PerformanceCounter> counters;

		private bool ok = true;

		public AppInstanceObserver(AppInstance appInstance)
		{
			#region Preconditions

			if (appInstance == null) throw new ArgumentNullException("app");

			#endregion

			this.appInstance = appInstance;

			var site = (Site)this.appInstance.Site;

			var path = $"/LM/W3SVC/{site.Id}/ROOT";

			var instanceName = path.Replace("/", "_");

			this.requestRate	= new PerformanceCounter("ASP.NET Applications", "Requests/Sec",		instanceName);
			this.errorRate		= new PerformanceCounter("ASP.NET Applications", "Errors Total/Sec",	instanceName);

			this.sendRate		= new PerformanceCounter("Web Service", "Bytes Sent/sec",			site.Name);
			this.recieveRate	= new PerformanceCounter("Web Service", "Bytes Received/sec",		site.Name);
			this.totalRequests	= new PerformanceCounter("Web Service", "Total Method Requests",	site.Name);
			this.uptime			= new PerformanceCounter("Web Service", "Service Uptime",			site.Name);

			this.counters		= new List<PerformanceCounter> { requestRate, errorRate, sendRate, recieveRate, totalRequests, uptime };

			// string curentWebApplicationInstanceName = System.Web.Hosting.HostingEnvironment.ApplicationID.Replace('/', '_');
		}

		public AppInstance AppInstance
		{
			get { return appInstance; }
		}

		public bool Faulted
		{
			get { return !ok; }
		}

		public Exception Fault { get; set; }

		public AppInstanceObservation Observe()
		{
			try
			{
				var observation = new AppInstanceObservation {
					AppInstance			= appInstance,
					RequestRateSample	= requestRate.NextSample(),
					ErrorRateSample		= errorRate.NextSample(),
					SendRateSample		= sendRate.NextSample(),
					ReceiveRateSample	= recieveRate.NextSample(),
					TotalRequestsSample = totalRequests.NextSample(),
					UptimeSample		= uptime.NextSample(),
					Date				= DateTime.UtcNow
				};

				this.ok = true;

				return observation;
			}
			catch(Exception ex)
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
			this.counters.ForEach(c => c.Dispose());
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