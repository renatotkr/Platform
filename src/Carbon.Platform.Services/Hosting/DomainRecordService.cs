using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    public class DomainRecordService : IDomainRecordService
    {
        private readonly PlatformDb db;
        private readonly IDomainService domainService;

        public DomainRecordService(PlatformDb db, IDomainService domainService)
        {
            this.db            = db ?? throw new ArgumentNullException(nameof(db));
            this.domainService = domainService;
        }
        
        public async Task<DomainRecord> GetAsync(long id)
        {
            return await db.DomainRecords.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.DomainRecord, id);
        }

        public async Task<IReadOnlyList<DomainRecord>> QueryAsync(string name, DomainRecordType type)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            var path = new DomainName(name).Path;

            var result = await db.DomainRecords.QueryAsync(
                expression: Expression.Conjunction(
                    Expression.Eq("path", path), 
                    Expression.Eq("type", type), 
                    Expression.IsNull("deleted")
                )
            );

            if (result.Count == 0)
            {
                // TODO: replace last label & check wildcard
            }

            return result;
        }

        public async Task<DomainRecord> CreateAsync(CreateDomainRecordRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var name = new DomainName(request.Name);

            var domain = await domainService.FindAsync(request.Name)
                ?? throw new ArgumentException($"domain {request.Name} not found");

            // TODO: Create a subdomain if it's a new record

            int? ttl = null;

            if (request.Ttl != null)
            {
                ttl = (int)request.Ttl.Value.TotalSeconds;
            }

            var record = new DomainRecord(
                id       : await db.DomainRecords.Sequence.NextAsync(),
                domainId : domain.Id,
                name     : request.Name,
                type     : request.Type,
                value    : request.Value,
                ttl      : ttl
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
            
            await db.DomainRecords.PatchAsync(request.Id, new[] {
                Change.Replace("value", request.Value)
            }, condition: Expression.IsNull("deleted"));
        }

        public async Task DeleteAsync(long id)
        {
            await db.DomainRecords.PatchAsync(id, new[] {
                Change.Replace("deleted", Expression.Func("NOW"))
            }, condition: Expression.IsNull("deleted"));
        }
    }
}