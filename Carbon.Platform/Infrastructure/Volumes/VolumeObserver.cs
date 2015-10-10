namespace Carbon.Platform
{
	using System;
	using System.Diagnostics;

	public class VolumeObserver : IDisposable
	{
		private readonly VolumeInfo volume;

		private readonly PerformanceCounter readTimeCounter;
		private readonly PerformanceCounter writeTimeCounter;
		private readonly PerformanceCounter readRateCounter;
		private readonly PerformanceCounter writeRateCounter;

		public VolumeObserver(VolumeInfo volume)
		{
			#region Preconditions

			if (volume == null) throw new ArgumentNullException("volume");

			#endregion

			this.volume = volume;

			string instanceName = volume.GetInstanceName();

			this.readTimeCounter =	new PerformanceCounter("LogicalDisk", "% Disk Read Time",		instanceName);
			this.writeTimeCounter = new PerformanceCounter("LogicalDisk", "% Disk Write Time",		instanceName);
			this.readRateCounter =	new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec",	instanceName);
			this.writeRateCounter = new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec",	instanceName);
		}

		public VolumeInfo Volume
		{
			get { return volume; }
		}

		public VolumeObservation Observe()
		{
			volume.Refresh();

			return new VolumeObservation {
				Volume = volume,
				Available = volume.Available,
				Size = volume.Size,
				Used = volume.Used,
				ReadTimeSample = readTimeCounter.NextSample(),
				WriteTimeSample = writeTimeCounter.NextSample(),
				ReadRateSample = readRateCounter.NextSample(),
				WriteRateSample = writeRateCounter.NextSample(),
				Date = DateTime.UtcNow
			};
		}

		public void Dispose()
		{
			readTimeCounter.Dispose();
			writeTimeCounter.Dispose();
			readRateCounter.Dispose();
			writeRateCounter.Dispose();
		}
	}
}
