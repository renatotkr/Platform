using System;
using System.Runtime.InteropServices;

namespace Carbon.Platform
{
    public readonly struct MemoryInfo
    {
        public MemoryInfo(long total, long available)
        {
            Total = total;
            Available = available;
        }

        public long Total { get; }

        public long Available { get; }
        
        public long Used => Total - Available;

        public static MemoryInfo Observe()
        {
            var memoryStatus = new MemoryStatusEx();

            if (GlobalMemoryStatusEx(memoryStatus))
            {
                return new MemoryInfo(
                    total     : (long)memoryStatus.ullTotalPhys,
                    available : (long)memoryStatus.ullAvailPhys
                );
            }

            throw new Exception("Error observing local memory");
        }

        #region Helpers

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MemoryStatusEx lpBuffer);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/aa366770(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class MemoryStatusEx
        {
            public uint dwLength;
            public uint dwMemoryLoad;

            // The amount of actual physical memory, in bytes.
            public ulong ullTotalPhys;

            // The amount of physical memory currently available, in bytes.
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