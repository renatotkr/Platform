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
        private readonly HostingDb db;

        public DomainService(HostingDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<Domain>> ListAsync(IEnvironment environment)
        {
            Validate.NotNull(environment, nameof(environment));

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
                var flags = DomainFlags.None;

                if (await HasSoaRecordAsync(name))
                {
                    flags |= DomainFlags.Authoritative; // it has an SOA record
                }
                else
                {
                    var response = await dns.QueryAsync(NameServer.Google, name.Name, DnsRecordType.A);
                        
                    if (response.Answers.Length == 0)
                    {
                        throw new Exception($"{name.Name} did not return any answer to a DNS query");
                    }
                }

                domain = await CreateAsync(new CreateDomainRequest(name.Name, flags: flags));
            }

            return domain;
        }

        public async Task<Domain> GetAsync(long id)
        {
            return await db.Domains.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Domain, id);
        }

        /// <summary>
        /// Binds a domain to an environment
        /// </summary>
        public async Task BindAsync(IDomain domain, IEnvironment environment)
        {
            #region Preconditions

            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            #endregion
            
            await db.Domains.PatchAsync(domain.Id, new[] {
                Change.Replace("environmentId", environment.Id),
                Change.Replace("ownerId",       environment.OwnerId)
            });            
        }

        public async Task UnbindAsync(IDomain domain)
        {
            #region Preconditions

            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            #endregion

            // TODO: Ensure the domain isn't managed...

            await db.Domains.PatchAsync(domain.Id, new[] {
                Change.Remove("environmentId"),
                Change.Remove("certificateId"),
                Change.Remove("originId"),
                Change.Remove("ownerId")
            });
        }

        public async Task<Domain> CreateAsync(CreateDomainRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

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

        #region Helpers

        private static readonly DnsClient dns = new DnsClient();

        private async Task<bool> HasSoaRecordAsync(DomainName domainName)
        {
            // Check if there's an SOA record (start of authority)
            var response = await dns.QueryAsync(NameServer.Google, domainName.Name, DnsRecordType.SOA);

            return response.Answers.Length > 0;
        }

        #endregion

    }
}