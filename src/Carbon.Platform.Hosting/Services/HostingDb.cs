using System;

namespace Carbon.Platform.Hosting
{
    using Data;

    public sealed class HostingDb
    {
        public HostingDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            Domains              = new Dataset<Domain,              long>(context);
            DomainAuthorizations = new Dataset<DomainAuthorization, long>(context);
            DomainRecords        = new Dataset<DomainRecord,        long>(context);
            DomainRegistrations  = new Dataset<DomainRegistration,  long>(context);
        }

        public IDbContext Context { get; }

        public const string Name = "Hosting";

        public Dataset<Domain,              long> Domains              { get; }
        public Dataset<DomainAuthorization, long> DomainAuthorizations { get; }
        public Dataset<DomainRecord,        long> DomainRecords        { get; }
        public Dataset<DomainRegistration,  long> DomainRegistrations  { get; }
    }
}