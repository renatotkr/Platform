using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

namespace Carbon.Platform
{
    public class Localhost
    {
        public Localhost(
            string name,
            IReadOnlyList<DriveInfo> drives, 
            IReadOnlyList<NetworkInterface> networkInterfaces
        )
        {
            Name              = name              ?? throw new ArgumentNullException(nameof(name));
            Drives            = drives            ?? throw new ArgumentNullException(nameof(drives));
            NetworkInterfaces = networkInterfaces ?? throw new ArgumentNullException(nameof(networkInterfaces));
        }

        public string Name { get; }

        public IReadOnlyList<DriveInfo> Drives { get; }

        public IReadOnlyList<NetworkInterface> NetworkInterfaces { get; }

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

                IReadOnlyList<DriveInfo> drives = Array.Empty<DriveInfo>();
                IReadOnlyList<NetworkInterface> networkInterfaces = Array.Empty<NetworkInterface>();

                try
                {
                    drives = GetFixedDrives().ToArray();
                    networkInterfaces = GetActiveNetworkInterfaces().ToArray();
                }
                catch { }

                current = new Localhost(
                    name              : Environment.MachineName,
                    drives            : drives,
                    networkInterfaces : networkInterfaces
                );

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

                if (ni.OperationalStatus == OperationalStatus.Up && physicalAddress.Length > 0 && physicalAddress != "00000000000000E0")
                {
                    yield return ni; 

                    /*
                    yield return new NetworkInterfaceInfo {
                        Description     = ni.Description,
                        Addresses       = GetIps(ni),
                        // InstanceName = InstanceName.FromDescription(ni.Description),
                        Mac = physicalAddress
                    };
                    */
                }
            }
        }

        /*
        public static void GetProcessors()
        {
            // TODO
        }
        */

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