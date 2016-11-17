using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Carbon.Platform
{
    using Computing;
    using Storage;

    public static class Localhost
    {
        private static Host current;

        private static readonly SemaphoreSlim mutex = new SemaphoreSlim(1);

        public static async Task<Host> GetAsync()
        {
            if (current != null) return current;

            await mutex.WaitAsync();

            try
            {
                current = new Host();

                try
                {
                    // current.MemoryTotal = MemoryInfo.Observe().Total;

                    current.Volumes = GetVolumes(current).ToList();
                }
                catch { }

                try
                {
                    current.NetworkInterfaces = GetActiveNetworkInterfaces().ToList();
                }
                catch { }

                try
                {
                    var ec2 = await Ec2Instance.FetchAsync().ConfigureAwait(false);

                    current.InstanceId = ec2.InstanceId;

                    // lookup zone...

                    current.Provider = ComputeProviderId.Amazon;
                    current.Location = ec2.Region + "/" + ec2.AvailabilityZone;
                    current.InstanceType = ec2.InstanceType;
                    current.PrivateIp = ec2.PrivateIp;

                    // current.ImageId = ec2.ImageId;

                    try
                    {
                        current.PublicIp = await Ec2Instance.GetPublicIpAsync().ConfigureAwait(false);
                    }
                    catch { }
                }
                catch (Exception ex)
                {
                    // Console.WriteLine("EC2Instance was not found" + ex.Message);
                }
            }
            finally
            {
                mutex.Release();
            }

            return current;
        }

        public static IEnumerable<NetworkInterface> GetActiveNetworkInterfaces()
        {
            var networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

            foreach (var ni in networkInterfaces)
            {
                if (ni.Description.StartsWith("Teredo")) continue;

                var physicalAddress = ni.GetPhysicalAddress().ToString();

                if (ni.OperationalStatus == OperationalStatus.Up && physicalAddress.Length > 0 && physicalAddress != "00000000000000E0")
                {
                    yield return new NetworkInterface {
                        // Description      = ni.Description,
                        Addresses       = GetIps(ni),
                        // InstanceName    = InstanceName.FromDescription(ni.Description),
                        Mac = physicalAddress
                    };
                }
            }
        }

        public static IEnumerable<VolumeInfo> GetVolumes(Host machine)
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == System.IO.DriveType.Fixed)
                {
                    yield return drive.ToVolumeInfo();
                }
            }
        }

        public static IPAddress[] GetIps(System.Net.NetworkInformation.NetworkInterface ni)
        {
            var ips = new List<IPAddress>();

            foreach (var ip in ni.GetIPProperties().UnicastAddresses)
            {
                if (IPAddress.IsLoopback(ip.Address)) continue;

                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips.Add(ip.Address);
                }
            }

            return ips.ToArray();
        }
    }
}