using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

namespace Carbon.Platform.Monitoring
{
    internal class Localhost
    {
        public Localhost(DriveInfo[] drives, NetworkInterface[] networkInterfaces)
        {
            Drives            = drives            ?? throw new ArgumentNullException(nameof(drives));
            NetworkInterfaces = networkInterfaces ?? throw new ArgumentNullException(nameof(networkInterfaces));
        }

        public DriveInfo[] Drives { get; }

        public NetworkInterface[] NetworkInterfaces { get; }

        private static Localhost current;

        private static readonly object _sync = new object();

        public static Localhost Get()
        {
            if (current != null)
            {
                return current;
            }

            lock (_sync)
            {
                if (current != null) return current;

                var drives            = Array.Empty<DriveInfo>();
                var networkInterfaces = Array.Empty<NetworkInterface>();

                try
                {
                    drives = GetFixedDrives().ToArray();
                    networkInterfaces = GetActiveNetworkInterfaces().ToArray();
                }
                catch { }

                current = new Localhost(drives, networkInterfaces);

                return current;
            }
        }

        public static IEnumerable<NetworkInterface> GetActiveNetworkInterfaces()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var ni in networkInterfaces)
            {
                if (ni.Description.StartsWith("Teredo")) continue;

                var physicalAddress = ni.GetPhysicalAddress().ToString();

                if (ni.OperationalStatus == OperationalStatus.Up
                    && physicalAddress.Length > 0
                    && physicalAddress != "00000000000000E0")
                {
                    yield return ni;
                }
            }
        }

        public static IEnumerable<DriveInfo> GetFixedDrives()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                {
                    yield return drive;
                }
            }
        }
    }
}