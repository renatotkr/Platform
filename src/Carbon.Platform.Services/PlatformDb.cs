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
            context.Types.TryAdd(new JsonObjectHandler());
            context.Types.TryAdd(new StringArrayHandler());
            context.Types.TryAdd(new MacAddressHandler());
            context.Types.TryAdd(new Int32ArrayHandler());
            context.Types.TryAdd(new Int64ArrayHandler());

            Context = context ?? throw new ArgumentNullException(nameof(context));

            // Environments -------------------------------------------------------------
            Environments         = new Dataset<EnvironmentInfo,           long>(context);
            Locations            = new Dataset<LocationInfo,              long>(context);

            EnvironmentUsers    = new Dataset<EnvironmentUser,    (long, long)>(context);
            EnvironmentPrograms = new Dataset<EnvironmentProgram, (long, long)>(context);

            // Computing ----------------------------------------------------------------
            Clusters              = new Dataset<Cluster,                  long>(context);
            Hosts                 = new Dataset<HostInfo,                 long>(context);
            HostTemplates         = new Dataset<HostTemplate,             long>(context);
            HostPrograms          = new Dataset<HostProgram,       (long, long)>(context);
            Images                = new Dataset<ImageInfo,                long>(context);
            MachineTypes          = new Dataset<MachineType,              long>(context);
            Programs              = new Dataset<ProgramInfo,              long>(context);
            ProgramReleases       = new Dataset<ProgramRelease,           long>(context);

            // Storage ------------------------------------------------------------------
            Buckets               = new Dataset<BucketInfo,               long>(context);
            Channels              = new Dataset<ChannelInfo,              long>(context);
            Queues                = new Dataset<QueueInfo,                long>(context);
            Volumes               = new Dataset<VolumeInfo,               long>(context);

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

        // Environments ---------------------------------------------------------
        public Dataset<EnvironmentInfo,         long> Environments       { get; }
        public Dataset<EnvironmentUser, (long, long)> EnvironmentUsers   { get; }

        public Dataset<LocationInfo,            long> Locations          { get; }

        public Dataset<EnvironmentProgram, (long environmentId, long programId)> EnvironmentPrograms { get; }

        // Computing ------------------------------------------------------------
        public Dataset<HostInfo,             long> Hosts                 { get; }
        public Dataset<Cluster,              long> Clusters              { get; }
        public Dataset<HostProgram,  (long hostId, long programId)> HostPrograms { get; }
        public Dataset<HostTemplate,         long> HostTemplates         { get; }
        public Dataset<HealthCheck,          long> HealthChecks          { get; }
        public Dataset<ImageInfo,            long> Images                { get; }
        public Dataset<MachineType,          long> MachineTypes          { get; }
        public Dataset<ProgramInfo,          long> Programs              { get; }
        public Dataset<ProgramRelease,       long> ProgramReleases       { get; }
        public Dataset<VolumeInfo,           long> Volumes               { get; }
                                                                           
        // Data -----------------------------------------------------------------
        public Dataset<BucketInfo,           long> Buckets               { get; }
        public Dataset<ChannelInfo,          long> Channels              { get; }
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
    }
}