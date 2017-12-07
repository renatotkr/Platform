using System;
using System.Collections.Generic;

namespace Carbon.Platform.Hosting
{
    public class SyncDomainRecordsResult
    {
        public SyncDomainRecordsResult(
            IReadOnlyList<DomainRecord> added, 
            IReadOnlyList<DomainRecord> removed)
        {
            Added   = added   ?? throw new ArgumentNullException(nameof(added));
            Removed = removed ?? throw new ArgumentNullException(nameof(removed));
        }

        public IReadOnlyList<DomainRecord> Added { get; }

        public IReadOnlyList<DomainRecord> Removed { get; }
    }
}