namespace Carbon.Platform
{
	using System;
	using System.Runtime.InteropServices;

	public class MemoryObserver
	{
		public MemoryObservation Observe()
		{
			var memoryStatus = new MemoryStatusEx();

			if (GlobalMemoryStatusEx(memoryStatus))
			{
				var observation = new MemoryObservation {
					Total = (long)memoryStatus.ullTotalPhys,
					Available = (long)memoryStatus.ullAvailPhys,
					Date = DateTime.UtcNow
				};

				observation.Used = observation.Total - observation.Available;

				return observation;
			}

			return null;
		}

		#region Helpers

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool GlobalMemoryStatusEx([In, Out] MemoryStatusEx lpBuffer);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private class MemoryStatusEx
		{
			public uint dwLength;
			public uint dwMemoryLoad;
			public ulong ullTotalPhys;
			public ulong ullAvailPhys;
			public ulong ullTotalPageFile;
			public ulong ullAvailPageFile;
			public ulong ullTotalVirtual;
			public ulong ullAvailVirtual;
			public ulong ullAvailExtendedVirtual;

			public MemoryStatusEx()
			{
				this.dwLength = (uint)Marshal.SizeOf(typeof(MemoryStatusEx));
			}
		}
		#endregion
	}
}
