using System;

using Carbon.Data.Sequences;

namespace Carbon.Platform
{
    using Security;

    public class PlatformClient : ApiBase
    {
        public PlatformClient(string host, ICredential credential)
            : this(host, new AccessTokenProvider(host, credential)) { }

        public PlatformClient(string host, IAccessTokenProvider accessTokenProvider)
            : base(host, accessTokenProvider)
        {
            Clusters     = new ClusterClient(this);
            Domains      = new DomainClient(this);
            Hosts        = new HostClient(this);
            Programs     = new ProgramClient(this);
            Repositories = new RepositoryClient(this);
            Environments = new EnvironmentClient(this);
            Events       = new EventClient(this);
            Exceptions   = new ExceptionClient(this);
            Deployments  = new DeploymentClient(this);
            Users        = new UsersClient(this);
        }
        
        public DeploymentClient Deployments { get; }

        public DomainClient Domains { get; }

        public ClusterClient Clusters { get; }

        public EnvironmentClient Environments { get; }

        public EventClient Events { get; }

        public ExceptionClient Exceptions { get; }

        public HostClient Hosts { get; }

        public ProgramClient Programs { get; }

        public RepositoryClient Repositories { get; }
        
        public UsersClient Users { get; }

        // Volumes

        // Networks
    }
}