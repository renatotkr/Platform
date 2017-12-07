using System;

using Amazon.Ec2;

namespace Carbon.Platform.Computing
{
    using static HostStatus;

    internal static class Ec2Extensions
    {
        public static HostStatus ToStatus(this InstanceState state)
        {
            switch (state.Name)
            {
                case "pending"       : return Pending;
                case "running"       : return Running;
                case "shutting-down" : return Terminating;
                case "terminated"    : return Terminated;
                case "stopping"      : return Stopping;
                case "stopped"       : return Stopped;

                default: throw new Exception("unexpected state:" + state.Name);
            }
        }
    }
}
