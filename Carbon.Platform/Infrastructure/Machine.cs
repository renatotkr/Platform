namespace Carbon.Platform
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Machine
    {
        private static MachineInfo current;

        private static readonly SemaphoreSlim mutex = new SemaphoreSlim(1);

        public static async Task<MachineInfo> GetAsync()
        {
            if (current != null) return current;

            await mutex.WaitAsync();

            try
            {
                current = new MachineInfo {
                    ProcessorCount = Environment.ProcessorCount
                };

                try
                {
                    current.MemoryTotal = LocalMemory.Observe().Total;
                    current.Ips = GetIps().Select(ip => ip.ToString()).ToArray();

                    current.VolumeNames = GetVolumes().Select(c => c.Name).ToArray();
                }
                catch { }

                try
                {
                    current.Macs = GetActiveNetworkInterfaces().Select(c => c.MacAddress).ToArray();
                }
                catch { }

                try
                {
                    var ec2 = await Ec2Instance.GetCurrent().ConfigureAwait(false);

                    current.InstanceId = ec2.InstanceId;
                    current.AvailabilityZone = ec2.AvailabilityZone;
                    current.InstanceType = ec2.InstanceType;
                    current.ImageId = ec2.ImageId;

                    if (ec2.PrivateIp != null)
                    {
                        current.PrivateIp = IPAddress.Parse(ec2.PrivateIp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EC2Instance was not found" + ex.Message);
                }

                current.Hash = CalculateHash(current);
            }
            finally
            {
                mutex.Release();
            }

            return current;
        }

        public static string CalculateHash(MachineInfo info)
        {
            // Combine the machine volume & mac ids to get it's hash
            // Future, just use the InstanceId

            var vols = GetVolumes().ToArray();

            if (info.Macs == null) throw new ArgumentNullException("No macs were found on the machine");

            var items = new List<string>();

            items.AddRange(info.Macs);

            var volIds = vols.Select(v => v.Guid).ToArray();

            if (volIds.Length != 0)
            {
                items.AddRange(volIds);
            }

            var text = string.Join("/", items.OrderBy(i => i));

            return Hash.ComputeSHA1(text).ToHex();
        }

        public static IEnumerable<NetworkInterfaceInfo> GetActiveNetworkInterfaces()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var ni in networkInterfaces)
            {
                if (ni.Description.StartsWith("Teredo")) continue;

                var macAddress = ni.GetPhysicalAddress().ToString();

                if (ni.OperationalStatus == OperationalStatus.Up && macAddress.Length > 0 && macAddress != "00000000000000E0")
                {
                    yield return new NetworkInterfaceInfo
                    {
                        Description = ni.Description,
                        IpAddresses = GetNetworkInterfaceIps(ni),
                        InstanceName = InstanceName.FromDescription(ni.Description),
                        MacAddress = macAddress
                    };
                }
            }
        }

        public static IEnumerable<VolumeInfo> GetVolumes()
        {
            foreach (var driveInfo in DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed))
            {
                yield return VolumeInfo.FromDriveInfo(driveInfo);
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

        public static IEnumerable<IPAddress> GetIps()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (var ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        // Skip the loopback address
                        if (IPAddress.IsLoopback(ip.Address)) continue;

                        // Skip non-IE4 addresses
                        if (ip.Address.AddressFamily != AddressFamily.InterNetwork) continue;

                        // Skip the 169 family
                        if (ip.Address.ToString().StartsWith("169.")) continue;

                        yield return ip.Address;
                    }
                }
            }
        }
    }
}