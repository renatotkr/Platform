using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface ICluster : IResource
    {
        long EnvironmentId { get; }
    }
}

// aws | TargetGroup for Application Load Balancer || AutoScalingGroup
// gcp | Instance Group for Load Balanced Backend

// Worker | Scales to queue pressure
// Webapp | Scales to requests

// X-Forwarded-For: <unverified IP(s)>, <immediate client IP>, <global forwarding rule external IP>, <proxies running in GCP>