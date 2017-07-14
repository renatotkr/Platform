using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    [Dataset("Domains")]
    public class DomainInfo : IDomain
    {
        public DomainInfo() { }

        public DomainInfo(
            long id,
            string name,
            long ownerId)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id      = id;
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId = ownerId;
        }

        [Member("id"), Key(sequenceName: "domainId")]
        public long Id { get; }

        // max-length = 253 characters
        // puny-code?
        [Member("name"), Unique]
        [StringLength(180)]
        public string Name { get; }

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

        [Member("registrarId")]
        public long RegistrarId { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Domain;

        #endregion

        #region Timestamps

        [Member("validated")]
        public DateTime? Validated { get; }

        [Member("expires"), Mutable]
        public DateTime Expires { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}