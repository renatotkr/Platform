using Carbon.Platform.Computing;

namespace Carbon.Platform.Management
{
    public static class AwsInstanceTypes
    {
        public static readonly IMachineType T2Medium = AwsInstanceType.Get("t2.medium");
        public static readonly IMachineType T2Large  = AwsInstanceType.Get("t2.large");
        public static readonly IMachineType P2XLarge = AwsInstanceType.Get("p2.xlarge"); // 0.900 /hour [demand] 0.45 [reserved] = ~ $648/m
    }
}
