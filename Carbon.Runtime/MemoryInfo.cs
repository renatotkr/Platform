using System;
using System.Runtime.InteropServices;

namespace Carbon.Platform
{
    public struct MemoryInfo
    {
        public long Available { get; set; }

        public long Used { get; set; }

        public long Total { get; set; }

        public static MemoryInfo Observe()
        {
            var memoryStatus = new MemoryStatusEx();

            if (GlobalMemoryStatusEx(memoryStatus))
            {
                var observation = new MemoryInfo {
                    Total = (long)memoryStatus.ullTotalPhys,
                    Available = (long)memoryStatus.ullAvailPhys
                };

                observation.Used = observation.Total - observation.Available;

                return observation;
            }


            throw new Exception("Could not observe local memory");
        }

        #region Helpers

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MemoryStatusEx lpBuffer);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
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
                this.dwLength = (uint)Marshal.SizeOf<MemoryStatusEx>();
            }
        }
        #endregion
    }
}