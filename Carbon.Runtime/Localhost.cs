using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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

                    /*
                    current.AvailabilityZone = ec2.AvailabilityZone;
                    current.InstanceType = ec2.InstanceType;
                    current.ImageId = ec2.ImageId;
                    */

                    if (ec2.PrivateIp != null)
                    {
                        current.PrivateIp = IPAddress.Parse(ec2.PrivateIp);
                    }

                    try
                    {
                        var publicIp = await Ec2Instance.GetPublicIpAsync().ConfigureAwait(false);

                        current.PublicIp = IPAddress.Parse(publicIp);
                    }
                    catch { }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EC2Instance was not found" + ex.Message);
                }
            }
            finally
            {
                mutex.Release();
            }

            return current;
        }

        public static IEnumerable<Networking.NetworkInterface> GetActiveNetworkInterfaces()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var ni in networkInterfaces)
            {
                if (ni.Description.StartsWith("Teredo")) continue;

                var physicalAddress = ni.GetPhysicalAddress().ToString();

                if (ni.OperationalStatus == OperationalStatus.Up && physicalAddress.Length > 0 && physicalAddress != "00000000000000E0")
                {
                    yield return new Networking.NetworkInterface {
                        // Description      = ni.Description,
                        Addresses       = GetNetworkInterfaceIps(ni),
                        // InstanceName    = InstanceName.FromDescription(ni.Description),
                        PhysicalAddress = physicalAddress
                    };
                }
            }
        }

        public static IEnumerable<VolumeInfo> GetVolumes(Host machine)
        {
            foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == System.IO.DriveType.Fixed))
            {
                yield return drive.ToVolumeInfo();
            }
        }

        public static IPAddress[] GetNetworkInterfaceIps(NetworkInterface ni)
        {
            return ni
                .GetIPProperties().UnicastAddresses
                .Where(ip => !IPAddress.IsLoopback(ip.Address) && ip.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(ip => ip.Address)
                .ToArray();
        }
    }
}