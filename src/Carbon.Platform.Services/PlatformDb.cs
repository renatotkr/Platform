using System;

namespace Carbon.Platform
{
    using Computing;
    using Data;
    using Environments;
    using Hosting;
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
            Locations            = new Dataset<LocationInfo,              long>(context);

            EnvironmentPrograms = new Dataset<EnvironmentProgram, (long, long)>(context);

            // Computing ----------------------------------------------------------------
            Clusters              = new Dataset<Cluster,                  long>(context);
            Hosts                 = new Dataset<HostInfo,                 long>(context);
            HostTemplates         = new Dataset<HostTemplate,             long>(context);
            Images                = new Dataset<Image,                    long>(context);
            MachineTypes          = new Dataset<MachineType,              long>(context);
            Programs              = new Dataset<ProgramInfo,              long>(context);
            ProgramReleases       = new Dataset<ProgramRelease,           long>(context);
            // Processes             = new Dataset<ProcessInfo,              long>(context);

            // Storage ------------------------------------------------------------------
            Buckets               = new Dataset<BucketInfo,               long>(context);
            Channels              = new Dataset<ChannelInfo,              long>(context);
            Databases             = new Dataset<DatabaseInfo,             long>(context);
            DatabaseEndpoints     = new Dataset<DatabaseEndpoint,         long>(context);
            DatabaseInstances     = new Dataset<DatabaseInstance,         long>(context);
            Queues                = new Dataset<QueueInfo,                long>(context);
            Volumes               = new Dataset<VolumeInfo,               long>(context);

            // Hosting ------------------------------------------------------------------
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
        }

        public IDbContext Context { get; }

        // Environment ----------------------------------------------------------
        public Dataset<EnvironmentInfo,      long> Environments          { get; }
        public Dataset<LocationInfo,         long> Locations             { get; }

        public Dataset<EnvironmentProgram, (long, long)> EnvironmentPrograms { get; }

        // Computing ------------------------------------------------------------
        public Dataset<HostInfo,             long> Hosts                 { get; }
        public Dataset<Cluster,              long> Clusters              { get; }
        public Dataset<HostTemplate,         long> HostTemplates         { get; }
        public Dataset<HealthCheck,          long> HealthChecks          { get; }
        public Dataset<Image,                long> Images                { get; }
        public Dataset<MachineType,          long> MachineTypes          { get; }
        // public Dataset<ProcessInfo,          long> Processes             { get; }
        public Dataset<ProgramInfo,          long> Programs              { get; }
        public Dataset<ProgramRelease,       long> ProgramReleases       { get; }
        public Dataset<VolumeInfo,           long> Volumes               { get; }
                                                                           
        // Data -----------------------------------------------------------------
        public Dataset<BucketInfo,           long> Buckets               { get; }
        public Dataset<ChannelInfo,          long> Channels              { get; }
        public Dataset<DatabaseInfo,         long> Databases             { get; }
        public Dataset<DatabaseEndpoint,     long> DatabaseEndpoints     { get; }
        public Dataset<DatabaseInstance,     long> DatabaseInstances     { get; }
        public Dataset<QueueInfo,            long> Queues                { get; }

        // Networks -------------------------------------------------------------
        public Dataset<NetworkInfo,          long> Networks              { get; }
        public Dataset<NetworkAddress,       long> NetworkAddresses      { get; }
        public Dataset<NetworkInterfaceInfo, long> NetworkInterfaces     { get; }
        public Dataset<NetworkSecurityGroup, long> NetworkSecurityGroups { get; }
        public Dataset<LoadBalancer,         long> LoadBalancers         { get; }
        public Dataset<LoadBalancerListener, long> LoadBalancerListeners { get; }
        public Dataset<LoadBalancerRule,     long> LoadBalancerRules     { get; }
        public Dataset<SubnetInfo,           long> Subnets               { get; }

        // Hosting --------------------------------------------------------------
        public Dataset<DomainInfo,           long> Domains               { get; }
    }
}