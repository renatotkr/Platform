using System;

namespace Carbon.Platform
{
    using Security;

    public class PlatformClient : ApiBase
    {
        public PlatformClient(Uri endpoint, ICredential credential)
            : base(endpoint, credential)
        {
            Clusters     = new ClusterClient(this);
            Hosts        = new HostClient(this);
            Programs     = new ProgramClient(this);
            Repositories = new RepositoryClient(this);
            Environments = new EnvironmentClient(this);
            Events       = new EventClient(this);
            Exceptions   = new ExceptionClient(this);
            Deployments  = new DeploymentClient(this);
        }

        // Builds

        // Buckets

        public DeploymentClient Deployments { get; }

        public ClusterClient Clusters { get; }

        public EnvironmentClient Environments { get; }

        public EventClient Events { get; }

        public ExceptionClient Exceptions { get; }

        public HostClient Hosts { get; }

        public ProgramClient Programs { get; }

        public RepositoryClient Repositories { get; }

        // Volumes

        // Networks

        // ...
    }
}