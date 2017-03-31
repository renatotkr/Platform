namespace Carbon.Platform.Computing
{
    public interface IBackend
    {
        long Id { get; }

        string Name { get; }

        int ProviderId { get; }
    }
}


// An internal or external facing proxy to access the backend

// AWS    : TargetGroup for Application Load Balancer
// Google : Instance Group for Load Balanced Backend

// AKA: Fleet, Cluster


// An webapp backend exposes an app/function as a load balanced worker or web service
// A worker backend manages one or more instances to handle a queue