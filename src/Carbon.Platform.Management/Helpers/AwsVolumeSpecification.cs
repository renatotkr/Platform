namespace Carbon.Platform.Management
{
    public class AwsVolumeSpecification
    {
        public AwsVolumeSpecification() { }

        public AwsVolumeSpecification(string deviceName, string type, long size)
        {
            DeviceName = deviceName;
            Type       = type;
            Size       = size;
        }

        public string DeviceName { get; set; }

        public long Size { get; set;  }

        public string Type { get; set;  }
    }
}

// Remain serializable with JSON