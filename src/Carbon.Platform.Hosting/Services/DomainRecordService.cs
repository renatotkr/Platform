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

            // order?

            return await db.DomainRecords.QueryAsync(
                And(Between("id", range.Start, range.End), IsNull("deleted"))
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
            #region Validation

            if (request.DomainId <= 0)
                throw new ArgumentException("Must be > 0", nameof(request.DomainId));

            if (string.IsNullOrEmpty(request.Name))
                throw new ArgumentException("Required", nameof(request.Name));

            if (request.Name.Length > 253)
                throw new ArgumentException("Must be 253 characters or fewer", nameof(request.Name));

            if (string.IsNullOrEmpty(request.Value))
                throw new ArgumentException("Required", nameof(request.Value));

            if (request.Ttl != null && request.Ttl.Value < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(request.Ttl), request.Ttl.Value.TotalSeconds, "Must be >= 0");

            #endregion

            var domain = await domainService.GetAsync(request.DomainId);

            string path = request.Name == "@"
                ? domain.Path
                : domain.Path + "/" + string.Join("/", request.Name.ToLower().Split('.').Reverse());
           
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

        public async Task<SyncDomainRecordsResult> SyncAsync(IDomain domain)
        {
            var added = new List<DomainRecord>();

            var name = DomainName.Parse(domain.Name);

            var records = await ListAsync(domain);

            if (!records.Any(r => r.Type == DnsRecordType.SOA))
            {
                added.AddRange(await SyncSet<SoaRecord>(domain, DnsRecordType.SOA));
            }

            if (!records.Any(r => r.Type == DnsRecordType.NS))
            {
                added.AddRange(await SyncSet<NSRecord>(domain, DnsRecordType.NS));
            }

            if (!records.Any(r => r.Type == DnsRecordType.MX))
            {
                added.AddRange(await SyncSet<MXRecord>(domain, DnsRecordType.MX));
            }

            return new SyncDomainRecordsResult(added, Array.Empty<DomainRecord>());
        }

        public async Task UpdateAsync(UpdateDomainRecordRequest request)
        {
            var record = GetAsync(request.Id);
            
            await db.DomainRecords.PatchAsync(record.Id, new[] {
                Change.Replace("value", request.Value.ToString())
            }, condition: IsNull("deleted"));
        }

        public async Task<bool> DeleteAsync(IDomainRecord record)
        {
            return await db.DomainRecords.PatchAsync(record.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }

        private static readonly DnsClient dns = new DnsClient();

        private async Task<IReadOnlyList<DomainRecord>> SyncSet<T>(IDomain domain, DnsRecordType type)
        {
            var response = await dns.QueryAsync(NameServer.Google, domain.Name, type);

            var added = new List<DomainRecord>();

            foreach (var record in response.Answers)
            {
                if (record.Data is T recordData)
                {
                    var newRecord = await CreateAsync(
                        new CreateDomainRecordRequest(
                            domainId : domain.Id,
                            name     : "@",
                            type     : record.Type,
                            value    : recordData.ToString(),
                            ttl      : TimeSpan.FromSeconds(record.Ttl)
                        )
                    );

                    added.Add(newRecord);
                }
            }

            return added;
        }
    }
}