namespace Carbon.Platform.Computing
{
    public static class HostTemplateProperties
    {
        public const string EBSOptimized     = "ebsOptimized";
        public const string SpotPrice        = "spotPrice";
        public const string KernelId         = "kernelId";
        public const string SecurityGroupIds = "securityGroupIds";
        public const string Monitoring       = "monitoring";

        // volumes [ { size: 100, type: "" } ]
        public const string Volumes          = "volumes";
        public const string Volume           = "volume";

        public const string IamRole          = "iamRole";
        public const string SubnetId         = "subnetId";
        public const string KeyName          = "keyName";
    }

    public class VolumeSpecification
    {
        public string DeviceName { get; set; }

        public long Size { get; set; }

        public string Type { get; set; }
    }
}