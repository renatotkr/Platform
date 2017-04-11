namespace Carbon.Platform.Computing
{
    public interface IHostGroup
    {
        long Id { get; }

        long EnvironmentId { get; }

        long LocationId { get; }
    }
}


// AWS    : TargetGroup for Application Load Balancer || AutoScalingGroup
// Google : Instance Group for Load Balanced Backend


// Backend Types
// Worker | Scales to queue pressure
// Webapp | Scales to requests

// What is the relationship between an application environment and backend?
// Does each backend have an environment
// Are they the same?

// X-Forwarded-For: <unverified IP(s)>, <immediate client IP>, <global forwarding rule external IP>, <proxies running in GCP>
