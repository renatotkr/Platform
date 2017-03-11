using System;

namespace Carbon.Platform
{
    using Apps;
    using Computing;
    using Data;
    using Networking;
    using Storage;
    using Versioning;

    public class PlatformDb
    {
        private readonly IDbContext context;

        public PlatformDb(IDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            // Ensure the db type handlers are registered
            context.Types.TryAdd(new SemanticVersionHandler());
            context.Types.TryAdd(new IPAddressHandler());
            context.Types.TryAdd(new IPAddressListHandler());
            context.Types.TryAdd(new ListenerHandler());
            context.Types.TryAdd(new HashHandler());
            context.Types.TryAdd(new ListenerListHandler());
            context.Types.TryAdd(new HealthCheckHandler());
            context.Types.TryAdd(new JsonObjectHandler());

            // Apps ------------------------------------------------------------------
            Apps         = new Dataset<App,         long>(context);
            AppInstances = new Dataset<AppInstance, (long, long)>(context);
            AppReleases  = new Dataset<AppRelease,  (long, SemanticVersion)>(context);
            AppEvents    = new Dataset<AppEvent,    long>(context);
            AppErrors    = new Dataset<AppError,    long>(context);

            // Backends --------------------------------------------------------------
            Backends     = new Dataset<Backend,     long>(context);

            // Computing & Storage ---------------------------------------------------
            Hosts             = new Dataset<HostInfo,   long>(context);
            Volumes           = new Dataset<VolumeInfo, long>(context);
            Images            = new Dataset<ImageInfo,  long>(context);

            // Networks --------------------------------------------------------------
            Networks          = new Dataset<NetworkInfo,          long>(context);
            NetworkAcls       = new Dataset<NetworkAcl,           long>(context);
            NetworkInterfaces = new Dataset<NetworkInterfaceInfo, long>(context);
            NetworkProxies    = new Dataset<NetworkProxy,         long>(context);
            NetworkRules      = new Dataset<NetworkRule,          long>(context);
            Subnets           = new Dataset<SubnetInfo,           long>(context);
        }

        public IDbContext Context => context;

        // Apps  -----------------------------------------------------------------
        public Dataset<App,         long>                    Apps         { get; }
        public Dataset<AppEvent,    long>                    AppEvents    { get; }
        public Dataset<AppInstance, (long, long)>            AppInstances { get; }
        public Dataset<AppRelease,  (long, SemanticVersion)> AppReleases  { get; }
        public Dataset<AppError,    long>                    AppErrors    { get; }

        // Backends --------------------------------------------------------------
        public Dataset<Backend, long> Backends { get; }

        // Computing & Storage ---------------------------------------------------                          
        public Dataset<HostInfo,   long> Hosts   { get; }
        public Dataset<ImageInfo,  long> Images  { get; }
        public Dataset<VolumeInfo, long> Volumes { get; }

        // Networks --------------------------------------------------------------
        public Dataset<NetworkInfo,          long> Networks          { get; }
        public Dataset<NetworkAcl,           long> NetworkAcls       { get; }
        public Dataset<NetworkInterfaceInfo, long> NetworkInterfaces { get; }
        public Dataset<NetworkProxy,         long> NetworkProxies    { get; }
        public Dataset<NetworkRule,          long> NetworkRules      { get; }
        public Dataset<SubnetInfo,           long> Subnets           { get; }
                                                    
    }
}