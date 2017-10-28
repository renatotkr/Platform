using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Net.Dns;
using Carbon.Platform.Environments;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    public sealed class DomainService : IDomainService
    {
        private static readonly DnsClient dns = new DnsClient();

        private readonly HostingDb db;

        public DomainService(HostingDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<Domain>> ListAsync(IEnvironment environment)
        {
            return db.Domains.QueryAsync(
                Expression.Eq("environmentId", environment.Id)
            );
        }

        public Task<Domain> FindAsync(DomainName name)
        {
            return db.Domains.QueryFirstOrDefaultAsync(
                Expression.Eq("path", name.Path)
            );
        }

        public async Task<Domain> GetAsync(DomainName name)
        {
            var domain = await FindAsync(name);

            if (domain == null)
            {
                try
                {
                    var response = await dns.QueryAsync(NameServer.Google, name.Name, DnsRecordType.SOA);

                    // Create it if there's an SOA record

                    if (response.Answers.Length == 0)
                    {
                        throw new Exception($"Domain {name} does not have a SOA record");
                    }

                    domain = await CreateAsync(new CreateDomainRequest(name.Name));
                }
                catch { }
            }

            return domain;
        }

        public async Task<Domain> GetAsync(long id)
        {
            return await db.Domains.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Domain, id);
        }

        public async Task UpdateAsync(UpdateDomainRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.RegistrationId != null)
            {
                // Ensure the registration exists?
            }

            await db.Domains.PatchAsync(request.Id, new[] {
                Change.Replace("registrationId", request.RegistrationId),
                Change.Replace("environmentId",  request.EnvironmentId),
                Change.Replace("certificateId",  request.CertificateId)
            });
        }

        public async Task ReleaseAsync(IDomain domain)
        {
            await db.Domains.PatchAsync(domain.Id, new[] {
                Change.Remove("environmentId"),
                Change.Remove("originId"),
                Change.Remove("ownerId")
            });
        }

        public async Task<Domain> CreateAsync(CreateDomainRequest request)
        {            
            var name = DomainName.Parse(request.Name);

            var flags = DomainFlags.None;

            if (name.Labels.Length == 1)
            {
                flags |= DomainFlags.Tld;
            }

            var domain = new Domain(
                id            : await db.Domains.Sequence.NextAsync(),
                name          : name.Name,
                ownerId       : request.OwnerId,
                environmentId : request.EnvironmentId,
                originId      : request.OriginId,
                flags         : flags
            );

            await db.Domains.InsertAsync(domain);

            return domain;
        }
    }
}