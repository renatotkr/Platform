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

            Domain parent = await GetParent(request.Name);

            var domain = new Domain(
                id       : await db.Domains.Sequence.NextAsync(),
                name     : request.Name.Name,
                ownerId  : request.OwnerId,
                parentId : parent?.Id ?? 0,
                flags    : flags
            );

            await db.Domains.InsertAsync(domain);

            return domain;
        }

        private async Task<Domain> GetParent(DomainName name)
        {
            // com
            // com/processor
            // com/processor/www
            var pathBuilder = new StringBuilder();

            Domain parent = null;

            var reversedLabels = name.Labels.Reverse().ToArray();

            if (reversedLabels.Length == 1) return null;

            // skip the last label

            for (var i = 0; i < (reversedLabels.Length - 1); i++)
            {
                if (i != 0)
                {
                    pathBuilder.Append('/');
                }

                pathBuilder.Append(reversedLabels[i]);
                
                parent = await db.Domains.QueryFirstOrDefaultAsync(
                    Expression.Eq("path", pathBuilder.ToString())
                );                
            }

            return parent;
        }
    }
}