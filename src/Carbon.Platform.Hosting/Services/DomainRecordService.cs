using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Net.Dns;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Hosting
{
    using static Expression;

    public sealed class DomainRecordService : IDomainRecordService
    {
        private readonly HostingDb db;
        private readonly IDomainService domainService;

        public DomainRecordService(HostingDb db, IDomainService domainService)
        {
            this.db            = db            ?? throw new ArgumentNullException(nameof(db));
            this.domainService = domainService ?? throw new ArgumentNullException(nameof(domainService));
        }
        
        public async Task<DomainRecord> GetAsync(long id)
        {
            return await db.DomainRecords.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.DomainRecord, id);
        }

        public async Task<IReadOnlyList<DomainRecord>> ListAsync(IDomain domain)
        {
            var range = ScopedId.GetRange(domain.Id);

            return await db.DomainRecords.QueryAsync(
                And(
                    Between("id", range.Start, range.End),
                    IsNull("deleted")
                )
            );
        }

        public async Task<IReadOnlyList<DomainRecord>> QueryAsync(DomainName name, DnsRecordType type)
        {
            var result = await db.DomainRecords.QueryAsync(
                Conjunction(
                    Eq("path", name.Path),
                    Eq("type", type), 
                    IsNull("deleted")
                )
            );

            if (result.Count == 0)
            {
                // Lookup wildcard records
                var path = name.GetPath(level: name.Labels.Length - 1) + "/*";

                result = await db.DomainRecords.QueryAsync(
                    Conjunction(
                        Eq("path", path),
                        Eq("type", type),
                        IsNull("deleted")
                    )
                );
            }

            return result;
        }

        public async Task<DomainRecord> CreateAsync(CreateDomainRecordRequest request)
        {            
            var domain = await domainService.GetAsync(request.DomainId);

            string path = null;

            if (request.Name == "@")
            {
                path = domain.Path;
            }
            else
            {
                path = domain.Path + "/" + string.Join("/", request.Name.ToLower().Split('.').Reverse());
            }

            int? ttl = null;

            if (request.Ttl != null)
            {
                ttl = (int)request.Ttl.Value.TotalSeconds;
            }

            var recordId = await DomainRecordId.NextAsync(db.Context, domain.Id);

            var record = new DomainRecord(
                id    : recordId,
                name  : request.Name,
                path  : path,
                type  : request.Type,
                value : request.Value,
                ttl   : ttl
            );
            
            await db.DomainRecords.InsertAsync(record);

            return record;
        }

        public async Task UpdateAsync(UpdateDomainRecordRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion
            
            // TODO: Verify value against Type

            await db.DomainRecords.PatchAsync(request.Id, new[] {
                Change.Replace("value", request.Value)
            }, condition: IsNull("deleted"));
        }

        public async Task DeleteAsync(long id)
        {
            await db.DomainRecords.PatchAsync(id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted"));
        }
    }
}