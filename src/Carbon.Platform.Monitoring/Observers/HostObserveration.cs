using System;

namespace Carbon.Platform.Monitoring
{
    public class HostObserveration
    {
        public HostObserveration(
            long hostId,
            AppObservation[] apps,
            MemoryStats memory,
            NetworkInterfaceObservation[] networkInterfaces,
            ProcessorObservation[] processors,
            VolumeObservation[] volumes,
            DateTime timestamp
        )
        {
            HostId            = hostId;
            Apps              = apps                ?? throw new ArgumentNullException(nameof(apps));
            Memory            = memory              ?? throw new ArgumentNullException(nameof(memory));
            NetworkInterfaces = networkInterfaces   ?? throw new ArgumentNullException(nameof(networkInterfaces));
            Processors        = processors          ?? throw new ArgumentNullException(nameof(processors));
            Volumes           = volumes             ?? throw new ArgumentNullException(nameof(volumes));
            Timestamp         = timestamp;
        }

        public long HostId { get; }

        public AppObservation[] Apps { get; }

        public MemoryStats Memory { get; }

        public ProcessorObservation[] Processors { get; }

        public NetworkInterfaceObservation[] NetworkInterfaces { get; }

        public VolumeObservation[] Volumes { get; }

        public DateTime Timestamp { get; }
    }
}