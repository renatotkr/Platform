using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Net.Dns;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    public class DomainService : IDomainService
    {
        private readonly HostingDb db;

        public DomainService(HostingDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Domain> FindAsync(DomainName name)
        {            
            return await db.Domains.QueryFirstOrDefaultAsync(
                Expression.Eq("path", name.Path)
            );
        }

        public async Task<Domain> GetAsync(long id)
        {
            return await db.Domains.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Domain, id);
        }

        public async Task UpdateAsync(UpdateDomainRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            if (request.RegistrationId != null)
            {
                // Ensure the registration exists
            }

            await db.Domains.PatchAsync(request.Id, new[] {
                Change.Replace("registrationId", request.RegistrationId),
                Change.Replace("certificateId",  request.CertificateId)
            });
        }

        public async Task<Domain> CreateAsync(CreateDomainRequest request)
        {
            var flags = request.Flags;
            
            if (request.Name.Labels.Length == 1)
            {
                flags |= DomainFlags.Tld;
            }


            var domain = new Domain(
                id       : await db.Domains.Sequence.NextAsync(),
                name     : request.Name.Name,
                ownerId  : request.OwnerId,
                flags    : flags
            );

            await db.Domains.InsertAsync(domain);

            return domain;
        }
    }
}