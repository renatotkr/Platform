using System;

namespace Carbon.Platform
{
    using CI;
    using Computing;
    using Data;
    using Hosting;
    using Iam;
    using Logging;
    using Networking;
    using Storage;

    public class PlatformDb
    {
        public PlatformDb(IDbContext context)
        {
            // Ensure the db type handlers are registered
            context.Types.TryAdd(new SemanticVersionHandler());
            context.Types.TryAdd(new IPAddressHandler());
            context.Types.TryAdd(new IPAddressArrayHandler());
            context.Types.TryAdd(new HashHandler());
            context.Types.TryAdd(new JsonObjectHandler());
            context.Types.TryAdd(new StringArrayHandler());
            context.Types.TryAdd(new MacAddressHandler());
            context.Types.TryAdd(new Int32ArrayHandler());
            context.Types.TryAdd(new Int64ArrayHandler());

            Context = context ?? throw new ArgumentNullException(nameof(context));

            // Environments -------------------------------------------------------------
            Environments         = new Dataset<EnvironmentInfo,           long>(context);
            EnvironmentLocations = new Dataset<EnvironmentLocation, (long, long)>(context);
            EnvironmentResources = new Dataset<EnvironmentResource,       long>(context);
            Locations            = new Dataset<LocationInfo,              long>(context);

            // Computing ----------------------------------------------------------------
            Hosts                 = new Dataset<HostInfo,                 long>(context);
            HostGroups            = new Dataset<HostGroup,                long>(context);
            HostTemplates         = new Dataset<HostTemplate,             long>(context);
            MachineImages         = new Dataset<MachineImageInfo,         long>(context);
            MachineTypes          = new Dataset<MachineType,              long>(context);
            Programs              = new Dataset<Program,                  long>(context);
            ProgramReleases       = new Dataset<ProgramRelease,           long>(context);

            // Storage ------------------------------------------------------------------
            Buckets               = new Dataset<BucketInfo,               long>(context);
            Channels              = new Dataset<ChannelInfo,              long>(context);
            Databases             = new Dataset<DatabaseInfo,             long>(context);
            DatabaseClusters      = new Dataset<DatabaseCluster,          long>(context);
            DatabaseEndpoints     = new Dataset<DatabaseEndpoint,         long>(context);
            DatabaseInstances     = new Dataset<DatabaseInstance,         long>(context);
            DataEncryptionKeys    = new Dataset<DataEncryptionKeyInfo,    long>(context); 
            EncryptionKeys        = new Dataset<EncryptionKeyInfo,        long>(context);
            Queues                = new Dataset<QueueInfo,                long>(context);
            Volumes               = new Dataset<VolumeInfo,               long>(context);

            // Hosting ------------------------------------------------------------------
            Certificates          = new Dataset<CertificateInfo,          long>(context);
            Domains               = new Dataset<DomainInfo,               long>(context);

            // Networks --------------------------------------------------------------
            Networks              = new Dataset<NetworkInfo,              long>(context);
            NetworkAddresses      = new Dataset<NetworkAddress,           long>(context);
            NetworkInterfaces     = new Dataset<NetworkInterfaceInfo,     long>(context);
            NetworkSecurityGroups = new Dataset<NetworkSecurityGroup,     long>(context);
            LoadBalancers         = new Dataset<LoadBalancer,             long>(context);
            LoadBalancerListeners = new Dataset<LoadBalancerListener,     long>(context);
            LoadBalancerRules     = new Dataset<LoadBalancerRule,         long>(context);
            Subnets               = new Dataset<SubnetInfo,               long>(context);

            // CI -----------------------------------------------------------------------
            Builds                = new Dataset<Build,                    long>(context);
            BuildArtifacts        = new Dataset<BuildArtifact,            long>(context);
            Deployments           = new Dataset<Deployment,               long>(context);
            DeploymentTargets     = new Dataset<DeploymentTarget, (long, long)>(context);

            // IAM ---------------------------------------------------------------------
            Users = new Dataset<User, long>(context);
            
            // Logging -----------------------------------------------------------------
            Activities = new Dataset<Activity, long>(context);
        }

        public IDbContext Context { get; }

        // Environment ----------------------------------------------------------------
        public Dataset<EnvironmentInfo,             long> Environments         { get; }
        public Dataset<EnvironmentLocation, (long, long)> EnvironmentLocations { get; }
        public Dataset<EnvironmentResource,         long> EnvironmentResources { get; }
        public Dataset<LocationInfo,                long> Locations            { get; }

        // Computing --------------------------------------------------------------
        public Dataset<HostInfo,              long> Hosts                  { get; }
        public Dataset<HostGroup,             long> HostGroups             { get; }
        public Dataset<HostTemplate,          long> HostTemplates          { get; }
        public Dataset<HealthCheck,           long> HealthChecks           { get; }
        public Dataset<MachineImageInfo,      long> MachineImages          { get; }
        public Dataset<MachineType,           long> MachineTypes           { get; }
        public Dataset<Program,               long> Programs               { get; }
        public Dataset<ProgramRelease,        long> ProgramReleases        { get; }
        public Dataset<VolumeInfo,            long> Volumes                { get; }
                                                                           
        // Data -------------------------------------------------------------------
        public Dataset<BucketInfo,            long> Buckets               { get; }
        public Dataset<ChannelInfo,           long> Channels              { get; }
        public Dataset<DatabaseInfo,          long> Databases             { get; }
        public Dataset<DatabaseCluster,       long> DatabaseClusters      { get; }
        public Dataset<DatabaseEndpoint,      long> DatabaseEndpoints     { get; }
        public Dataset<DatabaseInstance,      long> DatabaseInstances     { get; }
        public Dataset<DataEncryptionKeyInfo, long> DataEncryptionKeys    { get; }
        public Dataset<EncryptionKeyInfo,     long> EncryptionKeys        { get; }
        public Dataset<QueueInfo,             long> Queues                { get; }

        // Networks ---------------------------------------------------------------
        public Dataset<NetworkInfo,           long> Networks              { get; }
        public Dataset<NetworkAddress,        long> NetworkAddresses      { get; }
        public Dataset<NetworkInterfaceInfo,  long> NetworkInterfaces     { get; }
        public Dataset<NetworkSecurityGroup,  long> NetworkSecurityGroups { get; }
        public Dataset<LoadBalancer,          long> LoadBalancers         { get; }
        public Dataset<LoadBalancerListener,  long> LoadBalancerListeners { get; }
        public Dataset<LoadBalancerRule,      long> LoadBalancerRules     { get; }
        public Dataset<SubnetInfo,            long> Subnets               { get; }

        // Hosting ----------------------------------------------------------------
        public Dataset<CertificateInfo,       long> Certificates          { get; }
        public Dataset<DomainInfo,            long> Domains               { get; }

        // CI ---------------------------------------------------------------------
        public Dataset<Build, long>                    Builds             { get; }
        public Dataset<BuildArtifact, long>            BuildArtifacts     { get; }
        public Dataset<Deployment, long>               Deployments        { get; }
        public Dataset<DeploymentTarget, (long, long)> DeploymentTargets  { get; }

        // IAM -------------------------------------------------------------------
        public Dataset<Activity, long>                 Activities         { get; }
        public Dataset<User, long>                     Users              { get; }
    }
}